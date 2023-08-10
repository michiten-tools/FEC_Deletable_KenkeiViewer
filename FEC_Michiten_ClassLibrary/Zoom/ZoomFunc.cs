using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Michiten_ClassLibrary.Zoom
{
    public class ZoomFunc
    {
        PictureBox pbImg;

        Bitmap bmp;
        private Graphics graphics = null;
        private Matrix matrix;

        ImageAttributes ia = new ImageAttributes();

        private bool isDragging = false;
        private PointF tmpPoint = new PointF(0f, 0f);

        private float matW;
        private float matH;

        //Size initSize = new Size(2560, 1440);
        float initScale = 1.0f;

        public ZoomFunc(PictureBox pbImg)
        {
            this.pbImg = pbImg;
            this.pbImg.MouseWheel += new MouseEventHandler(pbImg_MouseWheel);
            this.pbImg.MouseDown += new MouseEventHandler(pbImg_MouseDown);
            this.pbImg.MouseMove += new MouseEventHandler(pbImg_MouseMove);
            this.pbImg.MouseUp += new MouseEventHandler(pbImg_MouseUp);

            InitImg();
        }

        private void InitImg()
        {
            if (graphics != null)
            {
                matrix = graphics.Transform;

                graphics.Dispose();
                graphics = null;
            }

            bmp = new Bitmap(pbImg.Width, pbImg.Height);
            pbImg.Image = bmp;

            graphics = Graphics.FromImage(pbImg.Image);

            if (matrix != null)
                graphics.Transform = matrix;

            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            DrawImage();
            graphics.Dispose();
        }


        public void DisposeImg()
        {
            if (bmp != null) bmp.Dispose();
            if (graphics != null) graphics.Dispose();
            if (matrix != null) matrix.Dispose();

        }
        private void DrawImage()
        {
            if (bmp == null)
                return;

            if (matrix != null)
                graphics.Transform = matrix;

            graphics.Clear(pbImg.BackColor);
            graphics.DrawImage(bmp,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);

            pbImg.Refresh();
        }

        public void SetImg(string imgPath)
        {
            if (bmp != null)
                bmp.Dispose();

            var tmp = pbImg.Image;

            bmp = new Bitmap(pbImg.Width, pbImg.Height);
            pbImg.Image = bmp;

            // FIXME: リソースリーク
            if (tmp != null)
                tmp.Dispose();

            graphics = Graphics.FromImage(pbImg.Image);
            bmp = new Bitmap(imgPath);

            if (matrix != null)
                matrix.Dispose();

            matW = (float)bmp.Width;
            matH = (float)bmp.Height;

            matrix = new Matrix();
            matrix.Scale((float)pbImg.Width / matW, (float)pbImg.Height / matH, MatrixOrder.Append);

            //initSize = new Size(bmp.Width, bmp.Height);
            initScale = (float)pbImg.Width / bmp.Width;

            graphics.Transform = matrix;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            DrawImage();

        }

        private void pbImg_MouseWheel(object sender, MouseEventArgs e)
        {
            if (matrix == null || pbImg.Image == null)
                return;

            matrix.Translate(-e.X, -e.Y, MatrixOrder.Append);
            if (e.Delta > 0)
            {
                if (matrix.Elements[0] < 4)
                    matrix.Scale(1.1f, 1.1f, MatrixOrder.Append);
            }
            else
            {
                if (matrix.Elements[0] > initScale)
                    matrix.Scale(1.0f / 1.1f, 1.0f / 1.1f, MatrixOrder.Append);
            }
            matrix.Translate(e.X, e.Y, MatrixOrder.Append);

            DrawImage();
        }

        private void pbImg_MouseDown(object sender, MouseEventArgs e)
        {
            if (matrix == null || pbImg.Image == null)
                return;

            if (e.Button.Equals(MouseButtons.Right))
            {
                matrix.Reset();
                matrix.Scale((float)pbImg.Width / matW, (float)pbImg.Height / matH, MatrixOrder.Append);

                DrawImage();
                return;
            }

            tmpPoint = new PointF(e.X, e.Y);

            isDragging = true;
        }

        private void pbImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (matrix == null || pbImg.Image == null)
                return;

            if (isDragging == true)
            {
                matrix.Translate(e.X - tmpPoint.X, e.Y - tmpPoint.Y, MatrixOrder.Append);

                DrawImage();

                tmpPoint = new PointF(e.X, e.Y);
            }
        }

        private void pbImg_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}
