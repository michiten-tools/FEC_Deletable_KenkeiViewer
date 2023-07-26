using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Util;
using FEC_Michiten_ClassLibrary.Zoom;
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

namespace FEC_Michiten_ClassLibrary.UserCtrl
{
    public partial class ImageView : UserControl
    {
        private ZoomFunc zoomFunc;

        private string rootPath;

        private SignItem currentItem;

        public ImageView()
        {
            InitializeComponent();

            zoomFunc = new ZoomFunc(picImage);
        }

        public void Init(string _rootPath)
        {
            rootPath = _rootPath;
            this.ParentForm.Resize += ParentForm_Resize;
        }

        private void ParentForm_Resize(object sender, EventArgs e)
        {
            if(currentItem == null)
                return;

            string img = Path.Combine(rootPath, Define.DirFrames, currentItem.ImageFileName);

            Size size;
            if (File.Exists(img))
            {
                size = UtilFunc.GetJpegFrameSize(img);

                // プロパティ取得失敗はデフォルト値
                if (size.Width == 0 || size.Height == 0)
                {
                    size.Width = 2560;
                    size.Height = 1440;
                }
            }
            else
            {
                // ファイルがないときはデフォルト値
                size = new Size(2560, 1440);
            }

            picImage.Height = picImage.Width * size.Height / size.Width;
            picImage.Location = new Point
            {
                X = picImage.Location.X,
                Y = (picImage.Parent.Height - picImage.Height) / 2,
            };

            SetImage(currentItem);
        }

        public void SetImage(SignItem item)
        {
            if (rootPath == null)
                throw new Exception("No Root Path");

            if(item == null)
            {
                picImage.Image.Dispose();
                return;
            }

            string img = Path.Combine(rootPath, Define.DirFrames, item.ImageFileName);

            if (File.Exists(img))
            {

                Log.Info("show img=" + img);

                if (!UtilFunc.CheckFrameComment(img))
                    return;

                Bitmap bmp = UtilFunc.GetResizeBitmap(picImage.Size, img);

                if (bmp == null)
                {
                    picImage.Image.Dispose();

                    UtilFunc.ErrMsg(Define.ErrMsgImgNotOpen);
                    Log.Error("file not found");
                    //ShowStatus(Define.ErrMsgImgNotOpen);
                }
                else
                {
                    if (picImage.Image != null)
                        picImage.Image.Dispose();

                    zoomFunc.SetImg(img);

                }
            }

            currentItem = item;

        }
    }
}
