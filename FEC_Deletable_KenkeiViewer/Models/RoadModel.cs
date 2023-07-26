
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Deletable_KenkeiViewer.Models
{
    public class RoadModel
    {
        public Guid RoadId { get; set; } = Guid.Empty;

        //public int Index { get; set; } = -1;
        public string Name { get; set; } = "";
        public string Define { get; set; } = "";

        public Color Color { get; set; } = Color.Magenta;

        public List<RectItem> Rects { get; set; } = new List<RectItem>();

        
    }

    public class RectItem
    {
        public Guid RectId { get; set; } = Guid.Empty;

        public int Index { get; set; }
        public double Width { get; set; } = 10;

        public int Selected { get; set; } = 0;
        public int Count { get; set; } = 0;

        public LatLng Src { get; set; } = null;
        public LatLng Dst { get; set; } = null;

        public List<LatLng> Points { get; set; } = new List<LatLng>();
    }
}
