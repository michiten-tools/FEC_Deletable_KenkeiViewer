using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using static System.Net.WebRequestMethods;

namespace FEC_Deletable_KenkeiViewer.Models
{
	public class SignItem
	{
		public Guid ItemID { get; set; }

		/// <summary>
		/// データ並び順（Grid行Indexとして利用）
		/// </summary>
		public int Index { get; set; }
		public string SourceFile { get; set; }
		public int CurrentFrame { get; set; }
		public string No { get; set; }
		public int Category { get; set; }
		public LongItemModel LongItem { get; set; } = new LongItemModel();
		public OffsetItem Offset { get; set; }
		public RoadName Name { get; set; } = new RoadName();
		public LatLng TimePlay { get; set; } = new LatLng();

		/// <summary>
		/// 登録付属物の緯度経度および時刻（現地時刻）
		/// </summary>
		public LatLng TimeTarget { get; set; } = new LatLng();

		/// <summary>
		/// オフセットなしオリジナル情報での登録付属物の緯度経度および時刻（現地時刻）
		/// </summary>
		public LatLng OrgTarget { get; set; } = new LatLng();
		//public string ImageFileName { get; set; }
		public Annotation Annotation { get; set; }
		public RenameModel Rename { get; set; } = new RenameModel();

		//public SignBoard SignBoard { get; set; } = new SignBoard();

		public List<DirectionItem> DirectionList { get; set; } = new List<DirectionItem>();

		///// <summary>
		///// 表示用種別名を返す
		///// </summary>
		//public string CategoryName
		//      {
		//	get { return CategoryDefine.GetCategoryStr(Category); }
		//      }

		/// <summary>
		/// 出力ファイル名
		/// </summary>
		public string ExportFileName { get; set; } = string.Empty;

		///// <summary>
		///// 長物の中間地点格納用
		///// </summary>
		//public List<LatLng> longItemMiddlePoint = new List<LatLng>();

		//public TrainItem TrainItemObj = new TrainItem();

		///// <summary>
		///// 長物か否かを判定
		///// </summary>
		///// <returns></returns>
		//public bool IsLongItemSrc()
		//{
		//	//カテゴリIDじゃなくて、どこかに長物有無を持ちたい
		//	if(Category == 7 || Category == 107)
		//		return true;
		//	else
		//		return false;
		//}

		/// <summary>
		/// モザイク画面用
		/// </summary>
		public string ImageFileName { get; set; }
    }

	public class TrainItem
	{

		public TrainPostItem PostA { get; set; }
        public TrainPostItem PostB { get; set; }

		public List<TrainPostItem> PostMList { get; set; } = new List<TrainPostItem>();

		public string NearStation { get; set; }

        public void Clear()
		{
			PostA = null;
            PostB = null;
			PostMList.Clear();
        }

		public List<TrainPostItem> AllPostItem()
		{
            List<TrainPostItem> list = new List<TrainPostItem>();
			if(PostA != null)
				list.Add(PostA);
            if (PostB != null)
                list.Add(PostB);
			list.AddRange(PostMList);
			return list;

        }

        public List<TrainPostItem> AllPostItemForLine()
        {
            List<TrainPostItem> list = new List<TrainPostItem>();
            if (PostA != null )
                list.Add(PostA);
            list.AddRange(PostMList);
            if (PostB != null)
                list.Add(PostB);
            return list;

        }

		public void AddPostM(TrainPostItem postM)
		{
			PostMList.Add(postM);

			if (PostA == null)
				return;

			double sumA = PostA.PostLatlng.Lat + PostA.PostLatlng.Lng;

			PostMList.Sort((x, y) =>
			{
				double sumX = Math.Abs(sumA - x.PostLatlng.Lat + x.PostLatlng.Lng);
				double sumY = Math.Abs(sumA - y.PostLatlng.Lat + y.PostLatlng.Lng);
				return (int)(sumY - sumX);
			});
		}


		public void SetNearStation(LatLng latLng)
		{
			string m = @"http://express.heartrails.com/api/json?method=getStations&x={0}&y={1}";
            string url = string.Format(m, new string[] { latLng.Lng.ToString(), latLng.Lat.ToString() });

            WebRequest request = WebRequest.Create(url);
            Stream rs = request.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(rs);

            JObject obj = JObject.Parse(sr.ReadToEnd());

			NearStation = obj["response"]["station"][0]["name"].ToString();

        }

    }

	public class TrainPostItem
	{
		public string PostNo { get; set; }
        public string PostCategory { get; set; }

        public LatLng PostLatlng { get; set; }

        public double Distance { get; set; }

    }

    public enum RoadType { None, National, Metropolis, City }
    public class RoadName
	{
		public RoadType Type { get; set; } = RoadType.None;
		public string LongName { get; set; } = "";
		public string ShortName { get; set; } = "";
	}


	public class DirectionItem
    {
		public string DirectionNo { get; set;}

		public DirectFlg DirectionType { get; set; }

		public string DirectionName { get; set; }

		public string Parent { get; set; }
		public string SourceFile { get; set; }
		public int CurrentFrame { get; set; }

		public Annotation Anno { get; set; }

		public SignBoard SignBoard { get; set; } = new SignBoard();

		public static string GetDirectionTypeString(DirectFlg df)
        {
			string ret = string.Empty;
            switch (df)
            {
                case DirectFlg.Front:
					ret = "正面";
                    break;
                case DirectFlg.Side:
					ret = "側面";
					break;
                case DirectFlg.Rear:
					ret = "背面";
					break;
                case DirectFlg.None:
                    ret = "指定なし";
                    break;
                default:
                    break;
            }
			return ret;
        }
		public static string GetDirectionTypeFlg(DirectFlg df)
		{
			string ret = string.Empty;
			switch (df)
			{
				case DirectFlg.Front:
					ret = "F";
					break;
				case DirectFlg.Side:
					ret = "S";
					break;
				case DirectFlg.Rear:
					ret = "R";
					break;
                case DirectFlg.None:
                    ret = "N";
                    break;
                default:
					break;
			}
			return ret;
		}
		public static DirectFlg GetDirectionTypeEnum(string str)
		{
            DirectFlg ret = DirectFlg.Front;
            switch (str)
            {
                case "正面":
                    ret = DirectFlg.Front;
                    break;
                case "側面":
                    ret = DirectFlg.Side;
                    break;
                case "背面":
                    ret = DirectFlg.Rear;
                    break;
				case "指定なし":
                    ret = DirectFlg.None;
                    break;
                default:
                    break;
            }
            return ret;
        }
	}

	public enum DirectFlg
    {
		Front,
		Side,
		Rear,
		None
    }

	public class LatLng
	{
		public double Lat { get; set; } = 0;

		public double Lng { get; set; } = 0;

		public DateTime Time { get; set; }


		public LatLng()
		{

		}

		public LatLng (double lat, double lng)
		{
			Lat = lat;
			Lng = lng;
		}



		/// <summary>
		/// Lat、Lngの両方に値がセットされているかを返す
		/// いずれかがゼロの場合はfalseで利用不可と判断する
		/// </summary>
		[JsonIgnore]
		public bool IsValidLatLng
        {
            get
            {
				if (Lat == 0) return false;
				if (Lng == 0) return false;

				return true;
            }
        }


		

	}


	/// <summary>
	/// アノテーション（矩形）
	/// </summary>
	public class Annotation
	{
		public float Ratio { get; set; }
		public Rectangle Rect { get; set; }
		public Color RectColor { get; set; }
		public float RectTihickness { get; set; }
	}

	public class LongItemModel
	{
		public Guid SrcID { get; set; }
		//public Guid DstID { get; set; }
	}

	/// <summary>
	/// 標識版クラス
	/// </summary>
	public class SignBoard
    {

		/// <summary>
		/// 施設に含まれる標識版数
		/// </summary>
		public int MaxNo { get; set; } = 0;

		/// <summary>
		/// 標識版設定済みフラグ
		/// </summary>
		public bool IsSignSetting { get; set; } = false;

		/// <summary>
		/// 標識版リスト
		/// </summary>
		public List<Sign> sign { get; set; } = new List<Sign>();

		/// <summary>
		/// 標識版クラス
		/// </summary>
		public class Sign
        {
            /// <summary>
            /// インデックス
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// 標識No
            /// </summary>
            public string SignNo { get; set; }
			/// <summary>
			/// 標識種別
			/// </summary>
			public string SignType { get; set; }

			/// <summary>
			/// 名称
			/// </summary>
			public string SignName { get; set; }

			/// <summary>
			/// 方向
			/// </summary>
			public string SignDirection { get; set; }

            /// <summary>
            /// 大型標識有無
            /// </summary>
            public bool SignLarge { get; set; }

            /// <summary>
            /// 補助有無
            /// </summary>
            public bool SignHojo { get; set; }

            /// <summary>
            /// 枠線倍率
            /// </summary>
            public float Ratio { get; set; }

			/// <summary>
			/// 枠線位置とサイズ
			/// </summary>
			public Rectangle Rect { get; set; }
			/// <summary>
			/// 枠線色
			/// </summary>
			public Color RectColor { get; set; }
			/// <summary>
			/// 枠線太さ
			/// </summary>
			public float RectTihickness { get; set; }
		}

	}
}
