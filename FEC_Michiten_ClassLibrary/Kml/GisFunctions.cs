using FEC_Michiten_ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Kml
{
	class GisFunctions
	{
		static double Radius_long = 6378137.0;
		static double Henpei = 1 / 298.257222101;
		static double f = Henpei;
		static double Radius_short = Radius_long * (1 - Henpei); // 6356752.314

		public static LatLng getGISBy60(string Latitude60, string Longitude60)
		{
			try
			{
				char[] _separator = { '°', '′', '"', '″', '"', '.', '．' };

				LatLng latlng = new LatLng();
				//latlng.Lat60 = Latitude60;
				//latlng.Lng60 = Longitude60;

				string _s = Latitude60.Trim();
				string[] Dms = _s.Split(_separator, StringSplitOptions.RemoveEmptyEntries);

				double dD = 0;
				double dM = 0;
				double dS = 0;
				double dSS = 0;
				if (!double.TryParse(Dms[0], out dD))
				{
					dD = 0;
				}
				if (1 < Dms.Length)
				{
					if (!double.TryParse(Dms[1], out dM))
					{
						dM = 0;
					}
				}
				if (2 < Dms.Length)
				{
					if (!double.TryParse(Dms[2], out dS))
					{
						dS = 0;
					}
				}
				if (3 < Dms.Length)
				{
					if (!double.TryParse(Dms[3], out dSS))
					{
						dSS = 0;
					}
				}
				else
				{
					dSS = 0;
				}

				double lat = dD + dM / Math.Pow(60, 1)
					+ double.Parse(string.Format("{0}.{1}", dS, dSS)) / Math.Pow(60, 2);
				latlng.Lat = lat;
				//latlng.LatStr = lat.ToString("#.00000");

				_s = Longitude60.Trim();
				Dms = _s.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
				if (!double.TryParse(Dms[0], out dD))
				{
					dD = 0;
				}
				if (1 < Dms.Length)
				{
					if (!double.TryParse(Dms[1], out dM))
					{
						dM = 0;
					}
				}
				if (2 < Dms.Length)
				{
					if (!double.TryParse(Dms[2], out dS))
					{
						dS = 0;
					}
				}
				if (3 < Dms.Length)
				{
					if (!double.TryParse(Dms[3], out dSS))
					{
						dSS = 0;
					}
				}
				else
				{
					dSS = 0;
				}
				double lon = dD + dM / Math.Pow(60, 1)
							+ double.Parse(string.Format("{0}.{1}", dS, dSS)) / Math.Pow(60, 2);

				latlng.Lng = lon;
				//latlng.LngStr = lon.ToString("#.00000");

				return latlng;
			}
			catch/* (Exception e) */
			{
				return null;
				//                throw CErr.HandleErrorChain(e);
			}
		}

		public static LatLng getGIS(string Latitude, string Longitude)
		{
			try
			{
				LatLng latlng = new LatLng();
				double dLat = 0;
				double dLon = 0;

				if (!double.TryParse(Latitude, out dLat))
				{
					dLat = 0;
				}
				if (!double.TryParse(Longitude, out dLon))
				{
					dLon = 0;
				}
				int iD = (int)Math.Floor(dLat);
				double dTmp = (dLat - iD) * 60;
				int iM = (int)Math.Floor(dTmp);
				dTmp = (dTmp - iM) * 60;
				//latlng.Lat60 = string.Format("{0}°{1}′{2}″", iD, iM, dTmp.ToString("F3").TrimEnd('0'));

				iD = (int)Math.Floor(dLon);
				dTmp = (dLon - iD) * 60;
				iM = (int)Math.Floor(dTmp);
				dTmp = (dTmp - iM) * 60;
				//latlng.Lng60 = string.Format("{0}°{1}′{2}″", iD, iM, dTmp.ToString("F3").TrimEnd('0'));
				string sPoint = string.Format(System.Globalization.CultureInfo.InvariantCulture, "POINT({0} {1})", dLon, dLat);

				latlng.Lat = dLat;
				//latlng.LatStr = dLat.ToString("#.00000");//Latitude;
				latlng.Lng = dLon;
				//latlng.LngStr = dLon.ToString("#.00000");// Longitude;

				return latlng;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				//throw CErr.HandleErrorChain(e);
				return null;
			}
		}

		public static double[] getDegLen(double x, double y)
		{
			if (x == 0)
				return new double[] { 0.0, y };
			else if (y == 0)
				return new double[] { 90.0, x };
			else
				return new double[] {
					radDo(Math.Atan(x / y)),
					Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))
				};
		}

		/// <summary>
		/// 方位角と距離から経緯度を求める
		/// http://tancro.e-central.tv/grandmaster/script/vincentyJS.html
		/// https://vldb.gsi.go.jp/sokuchi/surveycalc/surveycalc/bl2stf.html
		/// </summary>
		/// <param name="lat1"></param>
		/// <param name="lng1"></param>
		/// <param name="alpha12"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static double[] vincenty(LatLng latlng, double alpha12, double length)
		{
			double lat1 = doRad(latlng.Lat);
			double lng1 = doRad(latlng.Lng);
			alpha12 = doRad(alpha12);
			//length = length * 1000;

			var U1 = Math.Atan((1 - Henpei) * Math.Tan(lat1));
			var sigma1 = Math.Atan(Math.Tan(U1) / Math.Cos(alpha12));
			var alpha = Math.Asin(Math.Cos(U1) * Math.Sin(alpha12));
			var u2 = xy(Math.Cos(alpha), 2) * (xy(Radius_long, 2) - xy(Radius_short, 2)) / xy(Radius_short, 2);
			var A = 1 + (u2 / 16384) * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));
			var B = (u2 / 1024) * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));
			var sigma = length / Radius_short / A;

			double sigma0 = sigma;
			double dm2;
			do
			{
				sigma0 = sigma;
				dm2 = 2 * sigma1 + sigma;
				var x1 = Math.Cos(sigma) * (-1 + 2 * xy(Math.Cos(dm2), 2)) - B / 6 * Math.Cos(dm2) * (-3 + 4 * xy(Math.Sin(dm2), 2)) * (-3 + 4 * xy(Math.Cos(dm2), 2));
				var dSigma = B * Math.Sin(sigma) * (Math.Cos(dm2) + B / 4 * x1);
				sigma = length / Radius_short / A + dSigma;
			} while (Math.Abs(sigma0 - sigma) > 1e-9);

			var x = Math.Sin(U1) * Math.Cos(sigma) + Math.Cos(U1) * Math.Sin(sigma) * Math.Cos(alpha12);
			var y = (1 - Henpei) *
				xy(xy(Math.Sin(alpha), 2) + xy((Math.Sin(U1) * Math.Sin(sigma)) - (Math.Cos(U1) * Math.Cos(sigma) * Math.Cos(alpha12)), 2), (1.0 / 2.0));

			var lamda = Math.Sin(sigma) * Math.Sin(alpha12) / (Math.Cos(U1) * Math.Cos(sigma) - Math.Sin(U1) * Math.Sin(sigma) * Math.Cos(alpha12));
			lamda = Math.Atan(lamda);
			var C = (Henpei / 16) * xy(Math.Cos(alpha), 2) * (4 + Henpei * (4 - 3 * xy(Math.Cos(alpha), 2)));
			var z = Math.Cos(dm2) + C * Math.Cos(sigma) * (-1 + 2 * xy(Math.Cos(dm2), 2));
			var omega = lamda - (1 - C) * Henpei * Math.Sin(alpha) * (sigma + C * Math.Sin(sigma) * z);

			return new double[] { radDo(Math.Atan(x / y)), radDo(lng1 + omega) };
		}


		/// <summary>
		/// 2つの緯度経度の距離を方位を算出
		/// </summary>
		/// <param name="gisFront"></param>
		/// <param name="gisRear"></param>
		/// <returns></returns>
		public static double[] vincentyInv(LatLng front, LatLng rear)
		{
			var lat1 = doRad(front.Lat);// double.Parse(front.LatStr));
			var lon1 = doRad(front.Lng);// double.Parse(front.LngStr));
			var lat2 = doRad(rear.Lat);// double.Parse(rear.LatStr));
			var lon2 = doRad(rear.Lng);// double.Parse(rear.LngStr));

			var omega = lon2 - lon1;
			double tanU1 = (1 - f) * Math.Tan(lat1), cosU1 = 1 / Math.Sqrt((1 + tanU1 * tanU1)), sinU1 = tanU1 * cosU1;
			double tanU2 = (1 - f) * Math.Tan(lat2), cosU2 = 1 / Math.Sqrt((1 + tanU2 * tanU2)), sinU2 = tanU2 * cosU2;
			double lamda = omega, dLamda, count = 0;

			double sinLamda, cosLamda, sinSigma, cosSigma, sigma, cos2alpha, cos2sm;
			do
			{
				sinLamda = Math.Sin(lamda);
				cosLamda = Math.Cos(lamda);
				var sin2sigma = (cosU2 * sinLamda) * (cosU2 * sinLamda) + (cosU1 * sinU2 - sinU1 * cosU2 * cosLamda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLamda);

				if (sin2sigma < 0)
					return null;  // co-incident points

				sinSigma = Math.Sqrt(sin2sigma);
				cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLamda;
				sigma = Math.Atan2(sinSigma, cosSigma);
				var sinAlpha = cosU1 * cosU2 * sinLamda / sinSigma;
				cos2alpha = 1 - sinAlpha * sinAlpha;
				cos2sm = cosSigma - 2 * sinU1 * sinU2 / cos2alpha;

				if (double.IsNaN(cos2sm))
					cos2sm = 0;  // equatorial line: cos2alpha=0 (§6)

				var C = f / 16 * cos2alpha * (4 + f * (4 - 3 * cos2alpha));
				dLamda = lamda;
				lamda = omega + (1 - C) * f * sinAlpha * (sigma + C * sinSigma * (cos2sm + C * cosSigma * (-1 + 2 * cos2sm * cos2sm)));

				if (count++ > 10)
				{
					break;
				}
			} while (Math.Abs(lamda - dLamda) > 1e-12);

			var u2 = cos2alpha * (1 - (1 - f) * (1 - f)) / ((1 - f) * (1 - f));
			var A = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));
			var B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));
			var dSigma = B * sinSigma * (cos2sm + B / 4 * (cosSigma * (-1 + 2 * cos2sm * cos2sm)
			  - B / 6 * cos2sm * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2sm * cos2sm)));
			var sokuchisencyo = Radius_short * A * (sigma - dSigma);
			var alpha12 = Math.Atan2(cosU2 * sinLamda, cosU1 * sinU2 - sinU1 * cosU2 * cosLamda);
			alpha12 = radDo(alpha12);

			return new double[] { alpha12, sokuchisencyo };
		}


		private static double doRad(double a)
		{
			return a / 180 * Math.PI;
		}

		private static double radDo(double a)
		{
			return a * 180 / Math.PI;
		}

		private static double xy(double x, double y)
		{
			return Math.Pow(x, y);
		}


		/// <summary>
		/// 緯度経度・時刻から2点間の移動速度を算出
		/// </summary>
		/// <param name="src"></param>
		/// <param name="dst"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public static double getSpeed(LatLng src, LatLng dst, double distance)
		{
			if (src.Time.Year == 1 || dst.Time.Year == 1)
				return -1;

			if (distance < 0)
				return -2;

			TimeSpan ts = new TimeSpan();

			if (src.Time < dst.Time)
				ts = dst.Time - src.Time;
			else
				ts = src.Time - dst.Time;

			double sec = ts.TotalSeconds;

			return (distance / sec) * 3.6;
		}
	}
}
