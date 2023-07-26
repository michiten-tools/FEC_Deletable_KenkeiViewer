using FEC_Michiten_ClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Models
{
	public class KmlModel
	{
		public int No { get; set; }
		public DateTime When { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }

		/// <summary>
		/// 動画ファイル名（実際はkmlファイル名）
		/// </summary>
		public string movieModel { get; set; }

		/// <summary>
		/// 方向
		/// </summary>
		public int Direction { get; set; }

		/// <summary>
		/// 速度（時速：km/h）
		/// </summary>
		public double Spd { get; set; }

		/// <summary>
		/// データ形式（Google / 旧KML）
		/// </summary>
		public bool IsGoogleType { get; set; }


		/// <summary>
		/// 有効な値であるか？（緯度経度が両方ゼロではない）
		/// </summary>
		public bool IsValidLatLng
        {
            get
            {
				if (Lat == 0) return false;
				if (Lng == 0) return false;

				return true;
            }
        }

		public string LatString
        {
            get
            {
				return Lat.ToString(Define.LatLngFormat);
            }
        }

		public string LngString
        {
            get
            {
				return Lng.ToString(Define.LatLngFormat);
            }
        }

		public string WhenString
        {
			get
            {
				return When.ToString(Define.DateTimeFormat);
            }
        }

		public string DirectionString
        {
			get
            {
				return Direction.ToString();
            }
        }

		public string IsGoogleTypeString
        {
			get
            {
				return IsGoogleType.ToString();
			}
        }




		/// <summary>
		/// 1秒あたり移動距離(km）
		/// </summary>
		/// <returns></returns>
		private double GetDistanceParSec()
        {
			if(Spd == 0)
            {
				return 0;
            }
			// 時速を秒速に換算
			return Spd / 3600;
        }


		/// <summary>
		/// KMLが持つ速度(km/h）で指定時間（秒）移動した場合の距離（km/h）を返す
		/// </summary>
		/// <param name="moveTimeSec"></param>
		/// <returns></returns>
		public double GetDistance(double moveTimeSec)
        {
			return GetDistanceParSec() * moveTimeSec;
        }


		/// <summary>
		/// Coordinateモデルから新しくKmlModelを生成して返す
		/// </summary>
		/// <param name="coordinate"></param>
		/// <param name="no"></param>
		/// <returns></returns>
		public static KmlModel CreateByCoordinate(Coordinate coordinate, int no, string movieFile)
        {
			return new KmlModel()
			{
				No = no,
				When = coordinate.When,
				Lat = coordinate.Lat,
				Lng = coordinate.Lng,
				Direction = coordinate.Direction,
				Spd = coordinate.Spd,
				IsGoogleType = coordinate.IsGoogleType,
				movieModel = movieFile
			};
        }


		/// <summary>
		/// 2点間の経過秒数を返す
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public double GetDiffTotalSec(KmlModel target)
        {
			return (When - target.When).TotalSeconds;
        }

        
    }



	/// <summary>
	/// Google形式Kmlから読みとった1KML要素
	/// </summary>
	public class Coordinate
	{
		public DateTime When { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
		public double Spd { get; set; }
		public int Direction { get; set; }

		private bool _isGoogleType;


		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="isGoogleType"></param>
		public Coordinate(bool isGoogleType = true)
        {
			_isGoogleType = isGoogleType;
        }


		/// <summary>
		/// タイプに応じたインスタンスを返す（Google、旧KML）
		/// </summary>
		/// <param name="isGoogle"></param>
		/// <returns></returns>
		public static Coordinate CreateByType(bool isGoogle)
        {
			return new Coordinate(isGoogle);
		}


		/// <summary>
		/// データ形式（Google / 旧KML）
		/// </summary>
		public bool IsGoogleType
        {
            get
            {
				return _isGoogleType;
            }
        }

	}
}
