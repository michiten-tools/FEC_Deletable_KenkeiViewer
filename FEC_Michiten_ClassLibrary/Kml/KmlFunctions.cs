using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FEC_Michiten_ClassLibrary.Kml
{
	public class KmlFunctions
	{

        public delegate void GuiUpdateEventHandler(int count, int total, double length,EventArgs e);
        //public event GuiUpdateEventHandler GuiUpdateEvent;

        /// <summary>
        /// 開発作業用デバッグ出力の有無切替Flag
        /// </summary>
        private bool _debugPrintEnabled = false;


        public List<KmlModel> GetAllKmls(string file)
        {
            List<KmlModel> kmls = new List<KmlModel>();
            DirectoryInfo di = new DirectoryInfo(Directory.GetParent(file).FullName);

            FileInfo[] kmlFiles = di.GetFiles("*.kml");

            foreach (FileInfo item in kmlFiles)
            {
                List<Coordinate> coordinates = Read(item.FullName);
                if (coordinates == null)
                {
                    return kmls;
                }

                int no = 0;
                foreach (var coordinate in coordinates)
                {
                    //kmlファイル名を追加
                    //カーソル選択時の動画切替のため
                    kmls.Add(KmlModel.CreateByCoordinate(coordinate, no, item.FullName));
                    no++;
                }
            }
            return kmls;
        }

        public List<KmlModel> GetKmls(string file)
        {
            List<KmlModel> kmls = new List<KmlModel>();
            FileInfo fi = new FileInfo(file);

            List<Coordinate> coordinates = Read(fi.FullName);

            int no = 0;
            foreach (var coordinate in coordinates)
            {
                //kmlファイル名を追加
                //カーソル選択時の動画切替のため
                kmls.Add(KmlModel.CreateByCoordinate(coordinate, no, fi.FullName));
                no++;
            }
            return kmls;
        }


        public List<Coordinate> Read(string file)
        {
            List<Coordinate> res = new List<Coordinate>();

            bool google = false;

            // GoogleKMLか確認
            using (StreamReader check = new StreamReader(file))
            {
                string line = check.ReadLine();

                if (line.StartsWith("<?xml"))
                {
                    //Log.Info($"Read: GooglKML {file}");
                    google = true;
                }
                else
                {
                    //Log.Info($"Read: LegacyKML {file}");
                }
            }

            using (StreamReader sr = new StreamReader(file))
            {

                Coordinate add = Coordinate.CreateByType(google);
                bool when = false;
                bool coord = false;
                bool direction = false;
                string line;

                char[] separator = Define.KmlSeparator;
                while ((line = sr.ReadLine()) != null)
                {
                    // Google形式（というか本来のKML）
                    if (google)
                    {
                        // 緯度、経度、速度情報の取得（NaNが含まれる場合は抜ける）
                        if (line.Contains(Define.KmlLineDescription))
                        {
                            if (KmlHelper.IsContainNaN(line)) continue;

                            KmlHelper.SetAttribute(line, add);
                            coord = true;
                        }

                        // 方位情報の取得
                        if (line.Contains(Define.KmlLineStyleUrl))
                        {
                            KmlHelper.SetDirection(line, add);
                            direction = true;
                        }

                            // 時刻情報whenの取得
                        if (line.Contains(Define.KmlLineWhen))
                        {
                            string[] strs = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                            DateTime? localTime = DateTimeOffsetHelper.CreateLocalTimeFromKmlString($"{strs[3]}T{strs[4]}");
                            if (localTime == null) continue;

                            add.When = (DateTime)localTime;

                            when = true;
                        }
                    }
                    // Legacy
                    else
                    {
                        if (line.Contains(Define.KmlLineWhen))
                        {
                            string[] strs = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                            DateTime tmp;
                            if (!DateTime.TryParse($"{strs[2]} {strs[3]}", out tmp))
                            {
                                if (!DateTime.TryParse($"{strs[3]} {strs[4]}", out tmp))
                                    continue;
                            }

                            add.When = tmp.ToLocalTime();
                            when = true;
                        }

                        if (line.Contains(Define.KmlLineCoord))
                        {
                            string[] strs = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                            // Note: KML間の補間をするようにしたので、NaNは飛ばしてOK
                            if (strs[2].Equals("NaN") || strs[1].Equals("NaN"))
                                continue;

                            double val = -1;
                            if (double.TryParse(strs[2], out val))
                            {
                                add.Lat = val;

                                if (double.TryParse(strs[1], out val))
                                {
                                    add.Lng = val;
                                    coord = true;
                                }
                            }
                        }
                    }


                    // 1要素分の追加条件が成立してたらListに追加
                    if(google)
                    {
                        if (when && coord && direction)
                        {
                            res.Add(add);

                            add = Coordinate.CreateByType(google);
                            when = false;
                            coord = false;
                            direction = false;
                        }
                    }
                    else
                    {
                        if (when && coord)
                        {
                            res.Add(add);

                            add = Coordinate.CreateByType(google);
                            when = false;
                            coord = false;
                        }
                    }

                }
            }
            return res;
        }


        public int GetCurrentKml(List<KmlModel> kmls, DateTime targetTime)
        {
            foreach (var kml in kmls)
            {
                if (targetTime < kml.When)
                {
                    int index = kmls.IndexOf(kml);
                    return index;

                    // REVIEW: -1 をするべきか？
                    //if (index == 0)
                    //    return 0;
                    //else
                    //    return index - 1;
                }
            }
            return 0;
        }


        /// <summary>
        /// KMLから日時に該当する経緯度を取得
        /// 指定時間に近い2つのKMLから指定時間を考慮した補完を行った情報を返す
        /// </summary>
        /// <param name="kmls"></param>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public LatLng GetTargetLatLng(List<KmlModel> kmls, DateTime targetTime)
        {
            DebugPrint("-------");
            DebugPrint($",,{targetTime.ToString("yyyy/MM/dd HH:mm:ss.fff")},targetTime");

            int ave = 6;
            double lat = 0;
            double lng = 0;

            // 指定日時が保持しているKML先頭よりも過去の場合
            if (targetTime < kmls[0].When)
            {
                KmlModel prev = new KmlModel
                {
                    Lat = kmls[0].Lat - (kmls[1].Lat - kmls[0].Lat),
                    Lng = kmls[0].Lng - (kmls[1].Lng - kmls[0].Lng),
                    When = kmls[0].When - new TimeSpan(0, 0, 1),
                };

                DebugPrint($"kml[0].when: {kmls[0].When}");
                DebugPrint($"{prev.Lat.ToString("#.000000")},{prev.Lng.ToString("#.000000")},{prev.When.ToString("yyyy/MM/dd HH:mm:ss.fff")},prev:kml[-1]");
                DebugPrint($"{kmls[0].Lat.ToString("#.000000")},{kmls[0].Lng.ToString("#.000000")},{kmls[0].When.ToString("yyyy/MM/dd HH:mm:ss.fff")},next:kml[0]");
                DebugPrint($"{kmls[1].Lat.ToString("#.000000")},{kmls[1].Lng.ToString("#.000000")},{kmls[1].When.ToString("yyyy/MM/dd HH:mm:ss.fff")},next:kml[1] ");

                return ComplementKml(prev, kmls[0], targetTime);
            }

            // 指定日時より新しいKMLを見つけた場合
            foreach (var kml in kmls)
            {
                if (targetTime <= kml.When)
                {
                    int index = kmls.IndexOf(kml);
                    if (1 < index)
                    {
                        DebugInfo(kmls[index - 1], kmls[index], index);
                        return ComplementKml(kmls[index - 1], kmls[index], targetTime);
                    }
                    else
                    {
                        DebugInfo(kmls[0], kmls[1], 1);
                        return ComplementKml(kmls[0], kmls[1], targetTime);
                    }
                }
            }

            // 指定日時より新しいKMLが無かった場合
            if (kmls[kmls.Count - 1].When < targetTime)
            {
                if (kmls.Count >= ave)
                {
                    foreach (var i in Enumerable.Range(kmls.Count - ave, ave))
                    {
                        lat += kmls[i].Lat - kmls[i - 1].Lat;
                        lng += kmls[i].Lng - kmls[i - 1].Lng;
                        DebugPrint($"kmls[{i}] {kmls[i].Lat} {kmls[i].Lng}");
                        DebugPrint($"latlng {lat} {lng}");
                    }
                }
                lat = lat / (double)ave;
                lng = lng / (double)ave;
                DebugPrint($"latlng {lat} {lng}");

                lat += kmls[kmls.Count - 1].Lat;
                lng += kmls[kmls.Count - 1].Lng;
                DebugPrint($"latlng {lat} {lng}");

                KmlModel next = new KmlModel
                {
                    Lat = lat,
                    Lng = lng,
                    When = kmls[kmls.Count - 1].When + new TimeSpan(0, 0, 1),
                };

                DebugPrint($"{kmls[kmls.Count - 1].Lat.ToString("#.000000")},{kmls[kmls.Count - 1].Lng.ToString("#.000000")},{kmls[kmls.Count - 1].When.ToString("yyyy/MM/dd HH:mm:ss.fff")},prev:kml[{kmls.Count - 1}]");
                DebugPrint($"{next.Lat.ToString("#.000000")},{next.Lng.ToString("#.000000")},{next.When.ToString("yyyy/MM/dd HH:mm:ss.fff")},next:kml[{kmls.Count}]");

                return ComplementKml(kmls[kmls.Count - 1], next, targetTime);
            }
            return null;
        }



        /// <summary>
        /// デバッグ出力（Flagで出力無効化）
        /// </summary>
        /// <param name="msg"></param>
        public void DebugPrint(string msg)
        {
            if (!_debugPrintEnabled) return;

            Debug.WriteLine(msg);
        }


        public void DebugInfo(KmlModel prevKml, KmlModel nextKml, int nextIndex)
        {
            if (!_debugPrintEnabled) return;

            Debug.WriteLine($"kml[index].when: {nextKml.When}, {nextIndex}");
            Debug.WriteLine($"{prevKml.Lat.ToString("#.000000")},{prevKml.Lng.ToString("#.000000")},{prevKml.When.ToString("yyyy/MM/dd HH:mm:ss.fff")},prev:kml[{nextIndex - 1}]");
            Debug.WriteLine($"{nextKml.Lat.ToString("#.000000")},{nextKml.Lng.ToString("#.000000")},{nextKml.When.ToString("yyyy/MM/dd HH:mm:ss.fff")},next:kml[{nextIndex}]");
        }



        /// <summary>
        /// kmlの補間（内挿）
        ///  指定した2つのKML間を補完した新しいLatLngを生成して返す
        /// 　・緯度経度は2点間を調整
        /// 　・時間は動作再生をセット
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private LatLng ComplementKml(KmlModel prev, KmlModel next, DateTime time)
        {
            // next, prevのいずれかと時間が完全一致なら、そのまま元データでLatLng作成して返す
            if (next.When == time) return LatLng.CreateByKmlModel(next);
            if (prev.When == time) return LatLng.CreateByKmlModel(prev);

            // 指定時間に完全一致のKMLがない場合、以下コードにて2つのKML間を算出

            // 秒ピッタリでmsecがゼロの時、計算結果がゼロとなりPrevをそのまま返してしまう
            // そのため秒ピッタリの場合は1000secをセットして1秒後の計算を可能としている
            int diffMilisec = time.Millisecond == 0 ? 1000 : time.Millisecond;

            LatLng res = new LatLng
            {
                Lat = prev.Lat + ((next.Lat - prev.Lat) * diffMilisec / 1000),
                Lng = prev.Lng + ((next.Lng - prev.Lng) * diffMilisec / 1000),
                Time = time,
            };

            DebugPrint($"{res.Lat.ToString("#.000000")},{res.Lng.ToString("#.000000")},,Complement");

            return res;
        }


        /// <summary>
        /// オフセット設定を適用させた座標モデルを生成
        /// </summary>
        /// <param name="kmls"></param>
        /// <param name="targetTime"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public LatLng GetTargetLatLngWithOffset(List<KmlModel> kmls, DateTime targetTime, OffsetItem offset)
        {
            LatLng res = null;

            int ave = 6;
            double lat = 0;
            double lng = 0;

            if (targetTime < kmls[0].When)
            {
                KmlModel prev = new KmlModel
                {
                    Lat = kmls[0].Lat - (kmls[1].Lat - kmls[0].Lat),
                    Lng = kmls[0].Lng - (kmls[1].Lng - kmls[0].Lng),
                    When = kmls[0].When - new TimeSpan(0, 0, 1),
                };
                res = ComplementKml(prev, kmls[0], targetTime);
                return AddOffset(res, prev, kmls[0], offset);
            }

            foreach (var kml in kmls)
            {
                if (targetTime <= kml.When)
                {
                    int index = kmls.IndexOf(kml);
                    if (1 < index)
                    {
                        Debug.WriteLine($"kml[index].when: {kml.When}, {index}");
                        Debug.WriteLine($"{kmls[index - 1].Lat.ToString("#.000000")},{kmls[index - 1].Lng.ToString("#.000000")},{kmls[index - 1].When.ToString("yyyy/MM/dd HH:mm:ss.fff")},prev:kml[{index - 1}]");
                        Debug.WriteLine($"{kmls[index].Lat.ToString("#.000000")},{kmls[index].Lng.ToString("#.000000")},{kmls[index].When.ToString("yyyy/MM/dd HH:mm:ss.fff")},next:kml[{index}]");

                        res = ComplementKml(kmls[index - 1], kmls[index], targetTime);
                        return AddOffset(res, kmls[index - 1], kmls[index], offset);
                    }
                    else
                    {
                        res = ComplementKml(kmls[0], kmls[1], targetTime);
                        return AddOffset(res, kmls[0], kmls[1], offset);
                    }
                }
            }

            if (kmls[kmls.Count - 1].When < targetTime)
            {
                if (kmls.Count >= ave)
                {
                    foreach (var i in Enumerable.Range(kmls.Count - ave, ave))
                    {
                        lat += kmls[i].Lat - kmls[i - 1].Lat;
                        lng += kmls[i].Lng - kmls[i - 1].Lng;
                    }
                }
                lat = lat / (double)ave;
                lng = lng / (double)ave;

                lat += kmls[kmls.Count - 1].Lat;
                lng += kmls[kmls.Count - 1].Lng;

                KmlModel next = new KmlModel
                {
                    Lat = lat,
                    Lng = lng,
                    When = kmls[kmls.Count - 1].When + new TimeSpan(0, 0, 1),
                };
                res = ComplementKml(kmls[kmls.Count - 1], next, targetTime);
                return AddOffset(res, kmls[kmls.Count - 1], next, offset);
            }
            return null;
        }

        // NOTE: 2020-11-16 Offset
        private LatLng AddOffset(LatLng src, KmlModel prev, KmlModel next, OffsetItem offset)
        {
            if (offset == null)
                return src;

            if (offset.Selected == 5)
                return src;

            LatLng front = new LatLng
            {
                Lat = next.Lat,
                Lng = next.Lng,
            };
            LatLng rear = new LatLng
            {
                Lat = prev.Lat,
                Lng = prev.Lng,
            };

            double[] vals = GisFunctions.vincentyInv(front, rear);

            // left
            if (offset.Selected < 5)
            {
                double[] temp = GisFunctions.vincenty(src, vals[0] + 90, (double)offset.Left);
                src.Lat = temp[0];
                src.Lng = temp[1];
            }

            // right
            else if (5 < offset.Selected)
            {
                double[] temp = GisFunctions.vincenty(src, vals[0] - 90, (double)offset.Right);
                src.Lat = temp[0];
                src.Lng = temp[1];
            }

            return src;
        }

        ///// <summary>
        ///// NMEAからGoogleフォーマットでのKMLファイル出力
        ///// </summary>
        ///// <param name="dir"></param>
        //public void ExportKml(string dir, TimeZoneInfo timezone)
        //{
        //    var nmeaFiles = Directory.EnumerateFiles(dir, Define.NmeaFileExt, SearchOption.TopDirectoryOnly).ToList();

        //    double length = 0;
        //    int exportCount = 0;

        //    DateTimeOffsetHelper dtOffsetHelper = new DateTimeOffsetHelper(timezone);

        //    GuiUpdateEvent(exportCount, nmeaFiles.Count, length, null);
        //    foreach (var file in nmeaFiles)
        //    {
        //        // kenwood特有の行を削除
        //        string nmeaFile = Cleansing(file);

        //        // nmeaの読込み
        //        List<NmeaDetail> nmeaDetails = LoadNmea(nmeaFile, dtOffsetHelper);

        //        // save
        //        length += SaveKml(nmeaFile, nmeaDetails);

        //        // delete
        //        FileInfo info = new FileInfo(nmeaFile);
        //        info.Delete();
        //        while (info.Exists)
        //        {
        //            info.Refresh();
        //        }
        //        exportCount++;
        //        GuiUpdateEvent(exportCount, nmeaFiles.Count, length, null);
        //    }

        //    Log.Info($"{dir}: {nmeaFiles.Count} kml files, {length}km");
        //}

        //private string Cleansing(string src)
        //{
        //    string dst = Path.Combine(
        //        Directory.GetParent(src).ToString(),
        //        "_" + Path.GetFileNameWithoutExtension(src) + Path.GetExtension(src)
        //    );

        //    using (StreamReader sr = new StreamReader(src))
        //    using (StreamWriter sw = new StreamWriter(dst))
        //    {
        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            if (!line.StartsWith(Define.NmeaGTRIP) && !line.StartsWith(Define.NmeaJKDSA))
        //            {
        //                sw.WriteLine(line);
        //            }
        //        }
        //    }
        //    Log.Info($"Cleansing NMEA src = {src}");
        //    Log.Info($"Cleansing NMEA dst = {dst}");

        //    return dst;
        //}


//        /// <summary>
//        /// 指定NMEAファイルを読取りKML出力用データ群を生成して返す
//        /// </summary>
//        /// <param name="nmeaFile"></param>
//        /// <param name="dtOffsetHelper"></param>
//        /// <returns></returns>
//        private List<NmeaDetail> LoadNmea(string nmeaFile, DateTimeOffsetHelper dtOffsetHelper)
//        {
//            List<NmeaDetail> nmeaDetails = new List<NmeaDetail>();
//            using (StreamReader sr = new StreamReader(nmeaFile))
//            {
//                while (!sr.EndOfStream)
//                {
//                    string line = sr.ReadLine();

//                    // 基本情報行取込み（緯度、経度、方向、速度、日時）
//                    if (line.StartsWith(Define.NmeaGPRMC) || line.StartsWith(Define.NmeaGNRMC))
//                    {
//                        // 有効なデータであればNmeaDetail生成
//                        var nmeaRow = new NmeaRow(line);
//                        if (!nmeaRow.IsValidData)
//                        {
//                            Log.Warning(Define.WarnGprmcLength);
//                            continue;
//                        }
//                        NmeaDetail add = new NmeaDetail(nmeaRow);

//                        // 有効な日時文字列があれば日付セットしてListに追加
//                        if (nmeaRow.DateTimeSring != null)
//                        {
//                            add.When = dtOffsetHelper.CreateLocalOffset(nmeaRow.DateTimeSring);
//                        }
//                        nmeaDetails.Add(add);
//                        continue;
//                    }

//                    // alt:海抜属性行の取得（以前の行で追加済みの直近インスタンスへ値セット）
//                    if (line.StartsWith(Define.NmeaGPGGA))
//                    {
//                        double alt = 0;
//                        string[] strs = line.Split(',');
//                        if (strs.Length != 15)
//                            continue;

//                        if (!string.IsNullOrEmpty(strs[11]))
//                        {
//                            alt = double.Parse(strs[11]);
//                        }

//                        //nmeaDetails.Add(new NmeaDetail
//                        //{
//                        //    Lat = lat,
//                        //    Lon = lon,
//                        //    Alt = alt,
//                        //    Direction = dir,
//                        //    Speed = spd,
//                        //    When = when
//                        //});

//                        if(nmeaDetails.Count > 0)
//                        {
//                            nmeaDetails[nmeaDetails.Count - 1].Alt = alt;
//                        }
//                    }

//                }
//            }

//            return nmeaDetails;
//        }

//        private double SaveKml(string nmeaFile, List<NmeaDetail> nmeaDetails)
//        {
//            string kmlFile = Path.Combine(
//                Directory.GetParent(nmeaFile).ToString(),
//                Path.GetFileNameWithoutExtension(nmeaFile).Trim('_') + Define.KmlExt);

//            double length = 0;

//            using (StreamWriter sw = new StreamWriter(kmlFile))
//            {
//                StringBuilder sb = new StringBuilder();

//                sb.Append(
//$@"<?xml version=""1.0"" encoding=""UTF-8""?>
//<kml xmlns = ""http://earth.google.com/kml/2.1"">
//    <Document>
//        <name>{Path.GetFileNameWithoutExtension(nmeaFile).Trim('_')}</name>
//        <visibility>1</visibility>
//		<Style id=""style_id_speed1"">
//			<IconStyle>
//				<Icon>
//					<href>http://maps.google.com/mapfiles/kml/pal4/icon57.png</href>
//				</Icon>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc0"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-0.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc1"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-1.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc2"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-2.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc3"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-3.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc4"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-4.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc5"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-5.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc6"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-6.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc7"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-7.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc8"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-8.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc9"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-9.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc10"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-10.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc11"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-11.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc12"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-12.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc13"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-13.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc14"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-14.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Style id=""style_id_direc15"">
//			<IconStyle>
//				<Icon>
//					<href>http://earth.google.com/images/kml-icons/track-directional/track-15.png</href>
//				</Icon>
//				<scale>0.7</scale>
//			</IconStyle>
//		</Style>
//		<Folder>
//			<name>Point</name>
//");

//                // 同じ時刻を書き込まないため
//                DateTimeOffset cache = new DateTimeOffset(new DateTime(1970, 1, 1));

//                int count = 1;
//                foreach (var item in nmeaDetails)
//                {
//                    //Debug.WriteLine($"nmeaDetails[{nmeaDetails.IndexOf(item)}] {item.When}");

//                    if (item.When == cache)
//                        continue;

//                    //Debug.WriteLine($"nmeaDetails[{nmeaDetails.IndexOf(item)}] {item.When}");

//                    sb.Append(
//$@"			<Placemark>
//				<description><![CDATA[#{count++}<br/>Date= {item.When.ToString("yyyy/MM/dd")}<br/>Time= {item.When.ToString("HH:mm:ss")}<br/>Lat= {item.LatKmlFormat}<br/>Lon= {item.LonKmlFormat}<br/>Spd= {item.Speed.ToString("0.000")}km/h<br/>Alt= {item.Alt.ToString("#0.000")}<br/>]]></description>
//                <visibility>1</visibility>
//                <styleUrl>#style_id_direc{GetDirectionStr(item.Direction)}</styleUrl>
//				<Point>
//					<coordinates>{item.Lon},{item.Lat},{item.Alt.ToString("#0.000")}</coordinates>
//				</Point>
//				<TimeStamp><when>{item.When.ToString(Define.KmlWriteWhenFormat)}</when></TimeStamp>
//			</Placemark>
//");

//                    // 1個目ではない
//                    if (cache.Year != 1970)
//                    {
//                        double sec = (item.When - cache).TotalSeconds;
//                        //Debug.WriteLine(sec);

//                        length += (item.Speed / 3600) * sec;
//                        //Debug.WriteLine(length);
//                    }

//                    cache = item.When;
//                }

//                sb.Append(
//$@"		</Folder>
//	</Document>
//</kml>");

//                sw.WriteLine(sb.ToString());
//            }

//            Log.Info($"Save KML {kmlFile} {length}km");

//            return length;
//        }

        private string GetDirectionStr(double dir)
        {
            double range = 360 / 16;
            foreach (var count in Enumerable.Range(1, 16))
            {
                if (dir < range * count - range / 2)
                {
                    //Debug.WriteLine(count - 1);
                    return (count - 1).ToString();
                }
            }
            return "0";
        }


        /// <summary>
        /// 指定フォルダ内のKMLファイル一覧を取得して返す
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static List<string> GetKmlFileList(string root)
        {
            List<string> res = new List<string>();

            try
            {
                // KMLファイルを取得する
                res = Directory.EnumerateFiles(root, Define.KmlFileExt, SearchOption.TopDirectoryOnly).ToList();

                // ソートしておく
                res = res.OrderBy(x => Regex.Replace(x, @"[^0-9]", "")).ToList();
            }
            catch (Exception e)
            {
                Log.Error($"GetKmlFileList Error: {e.Message}");
            }

            return res;
        }
    }
}
