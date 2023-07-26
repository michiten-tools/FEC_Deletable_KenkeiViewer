using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Util
{
	public class Define
	{
		public const string LOG_DIR = "Log";

		public enum SearchCmd { Search, Clear }

		public enum BlnIcon { Info, Warn, Error }
		public enum PairsMode { Lot, Label }

		public enum FileProperty
		{
			FileSize = 1,
			Width = 316,//301,//295,
			Height = 314,//299,//293,
			FrameRate = 315,//300,//264,
			Dulation = 27,
			Comment = 24,
		}

		// webview
#if DEBUG
		public static string WebViewRuntimePath = Path.Combine(
			//$"{Environment.GetEnvironmentVariable("SystemDrive")}\\",
			Environment.CurrentDirectory,
			"fec-lib", "Microsoft.WebView2.FixedVersionRuntime.101.0.1210.47.x64");
#else
		public static string WebViewRuntimePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
			"fec", "Microsoft.WebView2.FixedVersionRuntime.101.0.1210.47.x64");
#endif

		// map
#if DEBUG
		public static string WebMapPath = Path.Combine(
			Directory.GetCurrentDirectory(),
			"web", "map.html");
		public static string RenameMapPath = Path.Combine(
			Directory.GetCurrentDirectory(),
			"rename", "map.html");
#else
		public static string WebMapPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
			"fec", "tetsuten", "web", "map.html");
		public static string RenameMapPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
			"fec", "tetsuten", "rename", "map.html");
#endif

		// Encoding
		public static readonly Encoding JIS = Encoding.GetEncoding("Shift_JIS");
		public static readonly Encoding UTF = Encoding.GetEncoding("UTF-8");

		// Extention, Filter
		public const string ExtCsv = ".csv";
		public const string ExtJpeg = ".jpeg";
		public const string ExtExcel = ".xlsx";

		public const string FilterExtCsv = "pairs.csv";
		public const string FilterExtJpeg = "*.jpeg";
		public const string FilterExtExcel = "*.xlsx";

		public const string FilterCsv = "csvファイル(*.csv)|*.csv";

		// dir
		public const string DirFrames = "frames";
        public const string DirSignFrames = "sign_frames";
        public const string DirExcel = "excel";

		// file
		public const string MapFile = "map.jpg";

		// string.Format
		public const string LatLngFormat = "#.00000000";
		public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";//.fff";
		public const string DateTimeFormat2 = "yyyy/MM/dd\nHH:mm:ss";//.fff";

		// Map Vals
		public const int MapZoomInit = 18;


		// Msg
		public const string MsgRun = "pairsファイル（.csv）を選択してくだい。";

		public const string MsgListUpdate = "表示中の内容が更新されますがよろしいですか？";

		public const string MsgOpenPairs = "pairsファイル（.csv）を選択してくだい。";
		public const string MsgOpenExcel = "エクセル調書ファイルがあるフォルダを選択してください。";

		public const string MsgLoadPairs = "pairsファイルを読み込みました。";
		public const string MsgLoadExcel = "エクセル調書のフォルダをセットしました。";

		public const string ErrMsgIncorrectSelection = "選択が正しくありません。";
		public const string ErrMsgFileNotFound = "選択したファイルが存在しません";
		public const string ErrMsgFileOpened = "paris.csvがExcel開かれています。";
		public const string ErrMsgDirectoryNotFound = "フォルダが存在しません。";
		public const string ErrMsgImgNotExists = "フォルダに画像ファイルが存在しません。";

		public const string ErrMsgImgNotOpen = "画像ファイルを開けませんでした。";
		public const string ErrMsgImgNotFound = "画像ファイルが見つかりませんでした。";

		public const string ErrMsgExcelDirIncorrection = "エクセル調書のフォルダが選択されていません。";
		public const string ErrMsgExcelNotFound = "選択されたフォルダにエクセル調書のファイルがありません。";

		public const string ErrMsgFileIsInUse = "該当のファイルは使用中です。";
		public const string ErrMsgFailedStartExcel = "エクセルの起動に失敗しました。";

        // KML Load Vals
        public static readonly char[] KmlSeparator = { '<', '>', 'T', 'Z', ' ' };
        public const string KmlLineWhen = "when";
        public const string KmlLineCoord = "<coord>";
        public const string KmlLineCoordinates = "<coordinates>";
        public const string KmlLineDescription = "<description>";
        public const string KmlLineStyleUrl = "<styleUrl>";
        public const string KmlCoordinates = "coordinates";
        public const string KmlWriteWhenS = "<when xmlns=\"\">";
        public const string KmlWriteWhenE = "</when>";
        public const string KmlWriteCoordS = "<coord>";
        public const string KmlWriteCorrdE = "</coord>";
        public const string KmlNmeaLoaded = "nmeaLoaded";
        public const string KmlKmlSaved = "kmlSaved";


        public const string MovFileExt = "*.mov";
        public const string KmlFileExt = "*.kml";
        public const string NmeaFileExt = "*.NMEA";
        public const string NmeaFileExt_ = "_*.NMEA";
        public const string JpegFileExt = "*.jpeg";
        public const string TextFileExt = "*.txt";
        public const string JsonFileExt = "*.json";
    }
}
