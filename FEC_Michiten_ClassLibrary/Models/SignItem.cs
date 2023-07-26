using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Models
{

	
	public class SignItem
	{
        [JsonIgnore]
        public int Index { get; set; }
        [JsonIgnore]
        public string SourceFile { get; set; }
        [JsonIgnore]
        public int CurrentFrame { get; set; }

        public string No { get; set; }
		[JsonIgnore]
        public int Category { get; set; }
        [JsonIgnore]
        public LongItemModel LongItem { get; set; } = new LongItemModel();
        [JsonIgnore]
        public LatLng TimePlay { get; set; } = new LatLng();
        [JsonIgnore]
        public LatLng TimeTarget { get; set; } = new LatLng();
        [JsonIgnore]
        public LatLng OrgTarget { get; set; } = new LatLng();
        [JsonIgnore]
        public string ImageFileName { get; set; }

        [JsonIgnore]
        public string Label { get; set; }

        [JsonIgnore]
        public Address address { get; set; } = new Address();

        [JsonIgnore]
        public List<SignBoard> SignList { get; set; } = new List<SignBoard>();
        /// <summary>
        /// 長物の中間地点格納用
        /// </summary>
        [JsonIgnore]
        public List<LatLngMiddlePoint> LongItemMiddlePoint { get; set; } = new List<LatLngMiddlePoint>();

        public string Memo { get; set; } = string.Empty;

		public bool IsSumi { get; set; } = false;
    }

	public class LatLng
	{
		public double Lat { get; set; } = 0;
		public double Lng { get; set; } = 0;
		public DateTime Time { get; set; }

        /// <summary>
		/// インスタンス生成（KMLモデルから）
		/// </summary>
		/// <param name="kml"></param>
		/// <returns></returns>
		public static LatLng CreateByKmlModel(KmlModel kml)
        {
            return new LatLng
            {
                Lat = kml.Lat,
                Lng = kml.Lng,
                Time = kml.When,
            };
        }
    }

	public class LatLngMiddlePoint : LatLng
	{
		public int Order { get; set; }
	}

	//public class NoteItem
 //   {
	//	public string Note { get; set; }
 //   }

	public class LongItemModel
	{
		public string SrcNoStr { get; set; }
		//public string DstNo { get; set; }
	}

	public class SignBoard
	{
        public string signNo { get; set; }
        public string signType { get; set; }
        public string signName { get; set; }
        public string signDirection { get; set; }
        public bool Hojo { get; set; }
        public string Filename { get; set; }
    }

    public class Address
    {
        public string Ken { get; set; }
        public string Sonota { get; set; }

    }
}
