using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Michiten_ClassLibrary.Map
{
	public class BorderSetting
	{
		public Color Color { get; set; } = Color.Black;
		public int Thickness { get; set; } = 1;
	}

	public class BorderRect
    {
		public Point Location { get; set; }
		public Size Size { get; set; }

		public PictureBox Top { get; set; } = new PictureBox();
		public PictureBox Left { get; set; } = new PictureBox();
		public PictureBox Buttom { get; set; } = new PictureBox();
		public PictureBox Right { get; set; } = new PictureBox();

		public bool Visible { get; set; } = false;
		public BorderSetting Setting { get; set; }

		public BorderRect(Point location, Size size)
		{
			this.Location = location;
			this.Size = size;

			SetBorderControl(new BorderSetting());
		}

		public Control[] GetControls()
		{
			return new Control[] { Top, Left, Buttom, Right };
		}

		public void SetVisible(bool visible)
		{
			Visible = visible;
			Top.Visible = visible;
			Left.Visible = visible;
			Buttom.Visible = visible;
			Right.Visible = visible;
		}

		public void ReDraw(Point location, Size size, BorderSetting border)
		{
			this.Location = location;
			this.Size = size;

			SetBorderControl(border);
		}

		public void SetBorderControl(BorderSetting border)
		{
			Top.Location = Location;
			Top.Size = new Size(Size.Width, border.Thickness);
			Top.BackColor = Color.Transparent;
			SetBorder(Top, border);

			Left.Location = Location;
			Left.Size = new Size(border.Thickness, Size.Height);
			Left.BackColor = Color.Transparent;
			SetBorder(Left, border);

			Buttom.Location = new Point(Location.X, Location.Y + Size.Height - border.Thickness);
			Buttom.Size = new Size(Size.Width, border.Thickness);
			Buttom.BackColor = Color.Transparent;
			SetBorder(Buttom, border);

			Right.Location = new Point(Location.X + Size.Width, Location.Y);
			Right.Size = new Size(border.Thickness, Size.Height);
			Right.BackColor = Color.Transparent;
			SetBorder(Right, border);
		}

		private void SetBorder(PictureBox box, BorderSetting border)
		{
			if (box.Width == 0 || box.Height == 0)
				return;

			Pen pen = new Pen(border.Color, border.Thickness);
			pen.DashStyle = DashStyle.Solid;

			Bitmap bmp = new Bitmap(box.Width, box.Height);
			Graphics g = Graphics.FromImage(bmp);
			Brush b = new SolidBrush(border.Color);
			g.FillRectangle(b, g.VisibleClipBounds);

			box.Image = bmp;

			b.Dispose();
			g.Dispose();
			pen.Dispose();
		}
	}
}
