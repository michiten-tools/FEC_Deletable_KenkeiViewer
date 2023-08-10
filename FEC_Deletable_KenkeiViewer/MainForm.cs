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

    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class MainForm : Form
    {

        private PairsFunc pFunc;

        //ビューア用pairsmodel
        private List<FEC_Michiten_ClassLibrary.Models.SignItem> dispList = new List<FEC_Michiten_ClassLibrary.Models.SignItem> ();
        //Aiちゃん用pairsmodel
        private Dictionary<string, Models.PairsModel> pairsModel = new Dictionary<string, Models.PairsModel>();
        private FEC_Michiten_ClassLibrary.Models.SignItem currentItem = null;
        private bool isOpen = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            //初期化だけ
            InitializeComponent();

            pFunc = new PairsFunc();
        }

        /// <summary>
        /// フォルダ読込
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bFolder_Click(object sender, EventArgs e)
        {
            dispList.Clear();
            pairsModel.Clear();
            currentItem = null;

            ///ダイアログ表示
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

            //フォルダ内にあるJSONを読込
            List<string> list = Directory.GetFiles(folderName).ToList().Where(w => w.EndsWith(".json")).ToList();

            //JSONファイルを読込
            list.ForEach(w =>
            {
                string strF = null;
                //ファイル読み込み
                using(StreamReader sr = new StreamReader(w))
                {
                    strF = sr.ReadToEnd();
                }

                try
                {
                    //デシリアライズ
                    Models.PairsModel tmp = JsonConvert.DeserializeObject<Models.PairsModel>(strF);
                    //if (pairsModel == null)
                    //{
                    //    //初回ならそのまま入れる
                    //    pairsModel = tmp;
                    //}
                    //else
                    //{

                    //    //２回目以降ならマージして入れる
                    //    pairsModel.Items = pairsModel.Items.Concat(tmp.Items).ToList();


                    //}
                    pairsModel.Add(w, tmp);

                }
                catch
                {
                    Log.Error("JSON file Deserialize fault " + w);
                    UtilFunc.ErrMsg("JSON読めませんでした。変なファイル入れたでしょ。\n" + w);
                }


            });


            //Aiちゃん用PairsModelからビューア用pairsModelを作る
            //ここ以降、その２つの二面持ち
            foreach (KeyValuePair<string, Models.PairsModel> pair in pairsModel)
            {
                dispList.AddRange(ConvertModel(pair));
            }

            //dispList = ConvertModel(pairsModel);

            //リストと地図と画像初期化
            listView.Init(dispList, folderName);
            imageView.Init(folderName);
            webViewCustom.AddIconTarget(dispList);
            isOpen = true;
        }

        /// <summary>
        /// AIちゃん用pairsmodelからビューア用pairsmodel作成
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private List<FEC_Michiten_ClassLibrary.Models.SignItem> ConvertModel(KeyValuePair<string, Models.PairsModel> pair)
        {

            //ビューア用pairsmodel
            List<FEC_Michiten_ClassLibrary.Models.SignItem> ret = new List<FEC_Michiten_ClassLibrary.Models.SignItem>();
            FEC_Michiten_ClassLibrary.Models.SignItem tmp = null;

            //AIちゃん用pairsmodel繰り返し
            foreach (Models.SignItem item in pair.Value.Items)
            {
                tmp = new FEC_Michiten_ClassLibrary.Models.SignItem();

                //一つ一つ要素に入れていく
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
                tmp.Memo = pair.Key;

                ret.Add(tmp);
            }

            return ret;

        }

        /// <summary>
        /// リスト選択時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_ListSelectChanged(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            var item = listView.GetSelectedItem();

            //地図とか画像を連動
            webViewCustom.SetCursor(item);
            webViewCustom.MoveMap(item);
            webViewCustom.OpenPopup(item);

            imageView.SetImage(item);
            currentItem = item;
        }

        /// <summary>
        /// 地図アイコン選択
        /// </summary>
        /// <param name="serialNo"></param>
        private void webViewCustom_WebViewIconClickEvent(string serialNo)
        {
            if (!isOpen)
                return;

            //リストとか画像と連動
            FEC_Michiten_ClassLibrary.Models.SignItem item = dispList.FirstOrDefault(x => x.No == serialNo);

            if (item == null)
                return;

            listView.SetSelectedItem(item);
            currentItem = item;
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDelete_Click(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            //フラグ立てるだけで実際には消さない
            //IsSumiを削除フラグとして流用
            currentItem.IsSumi = true;
            listView.dgvItemList.SelectedRows[0].DefaultCellStyle.BackColor = Color.Gray;
        }

        /// <summary>
        /// 削除しないボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDeleteBack_Click(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            //フラグ立てるだけで実際には消さない
            currentItem.IsSumi = false;
            listView.dgvItemList.SelectedRows[0].DefaultCellStyle.BackColor = Color.White;
        }

        /// <summary>
        /// 保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bSave_Click(object sender, EventArgs e)
        {
            if (!isOpen)
                return;

            //本当に消すから何回も聞く
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

            //これしないと画像が消せない
            imageView.SetImage(null);

            string imagePath;

            //リスト繰り返し
            foreach (var item in dispList)
            {

                Models.PairsModel tmpPm = pairsModel[item.Memo];

                Models.SignItem tmp = tmpPm.Items.FirstOrDefault(x => x.No == item.No);
                if (tmp == null)
                    continue;

                //IsSumiだったら消します
                if(item.IsSumi)
                {

                    //画像ファイルあれば削除
                    imagePath = Path.Combine(txtFolder.Text,Define.DirFrames, tmp.ExportFileName);
                    if(File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                    //リストから消すのは、Aiちゃん用pairsmodel、つまりJSONから消す
                    tmpPm.Items.Remove(tmp);
                }

            }

            //フォルダなければ作ります
            if (!Directory.Exists(Path.Combine(txtFolder.Text, "convert")))
            {
                Directory.CreateDirectory(Path.Combine(txtFolder.Text, "convert"));
            }

            foreach (KeyValuePair<string,  Models.PairsModel> pair in pairsModel)
            {
                //indexを振り直し、これしないとAIちゃんに戻したときによろしくないから
                pair.Value.Items.ForEach(x => x.Index = pair.Value.Items.IndexOf(x));

                //ファイル名はdelete.json
                string jsonSavePath = Path.Combine(txtFolder.Text, "convert", Path.GetFileName(pair.Key));

                //書き込み
                using (StreamWriter sw = new StreamWriter(jsonSavePath))
                {
                    sw.Write(JsonConvert.SerializeObject(pair.Value, Formatting.Indented));
                }
            }

            MessageBox.Show("消しました。");

            isOpen = false;
            listView.dgvItemList.Rows.Clear();
            txtFolder.Text = string.Empty;

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
