using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FEC_Michiten_ClassLibrary;
using FEC_Michiten_ClassLibrary.Pairs;
using FEC_Michiten_ClassLibrary.Util;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using FEC_Deletable_KenkeiViewer.Models;
using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.UserCtrl;
using static FEC_Michiten_ClassLibrary.Util.Define;

namespace FEC_Deletable_KenkeiViewer
{
    public partial class MainForm : Form
    {

        private PairsFunc pFunc;

        List<FEC_Michiten_ClassLibrary.Models.SignItem> dispList = null;
        Models.PairsModel pairsModel = null;
        FEC_Michiten_ClassLibrary.Models.SignItem currentItem = null;

        public MainForm()
        {
            InitializeComponent();

            pFunc = new PairsFunc();

            bFolder.Visible = true;

        }

        private void bFolder_Click(object sender, EventArgs e)
        {


            CommonOpenFileDialog dialog = new CommonOpenFileDialog()
            {
                Title = "JSON突っ込んであるのはどこよ？",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                // フォルダ選択モードにする
                IsFolderPicker = true,
            };

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                Log.Info("click cancel");
                return;
            }

            string folderName = dialog.FileName;
            txtFolder.Text = folderName;

            List<string> list = Directory.GetFiles(folderName).ToList().Where(w => w.EndsWith(".json")).ToList();
            pairsModel = null;

            list.ForEach(w =>
            {
                string strF = null;
                using(StreamReader sr = new StreamReader(w))
                {
                    strF = sr.ReadToEnd();
                }

                Models.PairsModel tmp = JsonConvert.DeserializeObject<Models.PairsModel>(strF);

                if (pairsModel == null)
                {
                    pairsModel = tmp;
                }
                else
                {

                    pairsModel.Items = pairsModel.Items.Concat(tmp.Items).ToList();


                }

            });

            


            dispList = ConvertModel(pairsModel);

            listView.Init(dispList, folderName);
            imageView.Init(folderName);

            webViewCustom.AddIconTarget(dispList);
            isOpen = true;
        }

        bool isOpen=false;
        private List<FEC_Michiten_ClassLibrary.Models.SignItem> ConvertModel(Models.PairsModel src)
        {
            List<FEC_Michiten_ClassLibrary.Models.SignItem> ret = new List<FEC_Michiten_ClassLibrary.Models.SignItem>();
            FEC_Michiten_ClassLibrary.Models.SignItem tmp = null;

            foreach (Models.SignItem item in src.Items)
            {
                tmp = new FEC_Michiten_ClassLibrary.Models.SignItem();

                tmp.Index = item.Index;
                tmp.No = item.No;
                tmp.Category = item.Category;
                tmp.TimePlay = new FEC_Michiten_ClassLibrary.Models.LatLng()
                {
                    Lat = item.TimePlay.Lat,
                    Lng = item.TimePlay.Lng,
                    Time = item.TimePlay.Time
                };
                tmp.TimeTarget = new FEC_Michiten_ClassLibrary.Models.LatLng()
                {
                    Lat = item.TimeTarget.Lat,
                    Lng = item.TimeTarget.Lng,
                    Time = item.TimeTarget.Time
                };

                tmp.Label = item.Rename.Rename;
                tmp.ImageFileName = item.ExportFileName;

                ret.Add(tmp);
            }

            return ret;

        }

        private void listView_ListSelectChanged(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            var item = listView.GetSelectedItem();

            webViewCustom.SetCursor(item);
            webViewCustom.MoveMap(item);
            webViewCustom.OpenPopup(item);

            imageView.SetImage(item);
            currentItem = item;
        }

        private void webViewCustom_WebViewIconClickEvent(string serialNo)
        {
            if (!isOpen)
                return;

            FEC_Michiten_ClassLibrary.Models.SignItem item = dispList.FirstOrDefault(x => x.No == serialNo);

            if (item == null)
                return;

            listView.SetSelectedItem(item);
            currentItem = item;
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            currentItem.IsSumi = true;
            listView.dgvItemList.SelectedRows[0].DefaultCellStyle.BackColor = Color.Gray;
        }

        private void bDeleteBack_Click(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            currentItem.IsSumi = false;
            listView.dgvItemList.SelectedRows[0].DefaultCellStyle.BackColor = Color.White;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            if (MessageBox.Show("消すよ？\nいいのね？","消すよ？", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                return;
            }

            if (MessageBox.Show("本当に消すよ？\n知らないよ？", "本気よ？", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                return;
            }

            if (MessageBox.Show("マジで消すよ？\n後悔するよ？", "マジよ？", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                return;
            }

            imageView.SetImage(null);

            string imagePath;
            foreach (var item in dispList)
            {
                Models.SignItem tmp = pairsModel.Items.FirstOrDefault(x => x.No == item.No);
                if (tmp == null)
                    continue;

                if(item.IsSumi)
                {
                    imagePath = Path.Combine(txtFolder.Text,Define.DirFrames, tmp.ExportFileName);
                    if(File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    pairsModel.Items.Remove(tmp);
                }


            }

            pairsModel.Items.ForEach(x => x.Index = pairsModel.Items.IndexOf(x));

            string jsonSavePath = Path.Combine(txtFolder.Text, "消したやつ.json");
            using(StreamWriter sw = new StreamWriter(jsonSavePath))
            {
                sw.Write(JsonConvert.SerializeObject(pairsModel, Formatting.Indented));
            }

            MessageBox.Show("消しました。いろいろ問");

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            string txt = textBox1.Text;

            var item = dispList.FirstOrDefault(x => x.No.Contains(txt) || x.Label.Contains(txt));

            if (item == null)
                return;

            currentItem = item;
            listView.SetSelectedItem(currentItem);
        }
    }


}
