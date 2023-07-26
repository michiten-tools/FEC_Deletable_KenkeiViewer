using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Models
{
    public class MapBounds
    {
        public LatLonModel NW { get; set; } = new LatLonModel();
        public LatLonModel NE { get; set; } = new LatLonModel();
        public LatLonModel SW { get; set; } = new LatLonModel();
        public LatLonModel SE { get; set; } = new LatLonModel();

        public MapBounds()
        {

        }

        public MapBounds(string bounds)
        {
            if (string.IsNullOrEmpty(bounds))
                return;

            string[] strs = bounds.Split(',');
            if (strs.Length != 8)
                return;

            NW = new LatLonModel(strs[0], strs[1]);
            NE = new LatLonModel(strs[2], strs[3]);
            SW = new LatLonModel(strs[4], strs[5]);
            SE = new LatLonModel(strs[6], strs[7]);
        }
    }

    public class LatLonModel
    {
        public double Lat { get; set; } = 0;
        public double Lon { get; set; } = 0;


        public LatLonModel()
        {

        }

        public LatLonModel(string lat, string lon)
        {
            if (!string.IsNullOrEmpty(lat))
            {
                double tmp = 0;
                double.TryParse(lat, out tmp);
                Lat = tmp;
            }
            else
            {
                Lat = 0;
            }

            if (!string.IsNullOrEmpty(lon))
            {
                double tmp = 0;
                double.TryParse(lon, out tmp);
                Lon = tmp;
            }
            else
            {
                Lon = 0;
            }
        }
    }
}
