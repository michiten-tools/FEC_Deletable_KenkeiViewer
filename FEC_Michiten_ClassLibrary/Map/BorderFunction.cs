using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Michiten_ClassLibrary.Map
{
    public class BorderFunction
    {
        WebView2 webView;

        public BorderRect Border;
        public BorderSetting Setting = new BorderSetting();

        public BorderFunction(WebView2 webView)
        {
            this.webView = webView;

            InitBorder();
        }

        public void InitBorder()
        {
            Border = new BorderRect(new Point(0, 0), new Size(100, 100));
            Setting = new BorderSetting { Color = Color.Red, Thickness = 3 };

            Control[] rectDachControls = Border.GetControls();
            foreach (var c in rectDachControls)
            {
                webView.Controls.Add(c);
                c.BringToFront();
            }
            Border.SetVisible(false);
        }

        public void DrawBorder(Point src, Point dst)
        {
            Size size = new Size(dst.X - src.X - 3, dst.Y - src.Y);
            Border.ReDraw(src, size, Setting);
        }

        public void SetBorderVisible(bool visible)
        {
            Border.SetVisible(visible);
        }
    }
}
