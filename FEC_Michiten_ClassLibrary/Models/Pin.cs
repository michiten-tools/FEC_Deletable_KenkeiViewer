using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Models
{
	public class Pin
	{
		public string No { get; set; }
		public string Lat { get; set; }
		public string Lng { get; set; }
		public string When { get; set; }

        public string Direction { get; set; }
        public string IsGoogleType { get; set; }

        /// <summary>
        /// 動画ファイル（実際に入るのはkml）
        /// </summary>
        public string movieFile { get; set; }

        public static Pin CreateByKmlModel(KmlModel kml)
        {
            return new Pin()
            {
                Lat = kml.LatString,
                Lng = kml.LngString,
                When = kml.WhenString,
                Direction = kml.DirectionString,
                IsGoogleType = kml.IsGoogleTypeString,
                movieFile = kml.movieModel
            };
        }
    }
}
