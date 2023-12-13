using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Util;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FEC_Michiten_ClassLibrary.Map.JsToCs;
using static FEC_Michiten_ClassLibrary.Util.Define;

namespace FEC_Michiten_ClassLibrary.Map
{
	public class MapFunc
	{
		WebView2 webView;

		// 表示する施設
		public TargetListsModel targetLists;

		// 表示の四隅座標
		public MapBounds mapBounds = new MapBounds();

		public PairsMode Mode = PairsMode.Lot;

        public delegate void IconClickEventHandler(string serialNo);
        public event IconClickEventHandler IconClickEvent;

        public delegate void DClickEventHandler(string serialNo);
		public event DClickEventHandler DClickEvent;

		public delegate void MapBoundsEventHandler(string bounds);
		public event MapBoundsEventHandler MapBoundsEvent;

        public delegate void ExcelClickEventHandler(string serialNo);
        public event ExcelClickEventHandler ExcelClickEvent;

        public delegate void KmlIconClickEventHandler(string datetimeStr, string kmlFile);
        public event KmlIconClickEventHandler KmlIconClickEvent;

        public MapFunc(WebView2 webView)
		{
			this.webView = webView;

			// C#とJSのパイプ準備
			var jsClass = new JsToCs();
			jsClass.JsIconDClickEvent += Js_IconDClickEvent;
			jsClass.JsGetMapBoundsEvent += Js_GetMapBoundsEvent;
            jsClass.JsIconClickEvent += Js_iconClickEvent;
			jsClass.JsExcelClickEvent += Js_ExcelClickEvent;
            jsClass.JsKmlClickEvent += Js_KmlClickEvent;

            webView.CoreWebView2?.AddHostObjectToScript("class", jsClass);
        }

		private void Js_iconClickEvent(string serialNo)
		{
			IconClickEvent?.Invoke(serialNo);
		}

		// JSからのイベント処理
		private void Js_IconDClickEvent(string serialNo)
        {
			DClickEvent?.Invoke(serialNo);
        }

		private void Js_GetMapBoundsEvent(string bounds)
        {
			MapBoundsEvent?.Invoke(bounds);
		}

        private void Js_ExcelClickEvent(string serialNo)
        {
            ExcelClickEvent?.Invoke(serialNo);
        }

        /// <summary>
        /// Js側でKMLアイコンクリックで呼ばれる -> 利用側への通知イベント発火
        /// </summary>
        /// <param name="index"></param>
		/// <param name="kmlFile">kmlファイル名</param>
        private void Js_KmlClickEvent(string datetimeStr, string kmlFile)
        {
            if (KmlIconClickEvent is null) return;

            KmlIconClickEvent(datetimeStr, kmlFile);
        }

        /// <summary>
        /// 振り分け
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public TargetListsModel AllocateTargets(List<SignItem> items)
		{

			TargetListsModel res = new TargetListsModel();
			foreach (var item in items)
			{
				if (item.Category == 1)
					res.TypeA.Add(item);
				if (item.Category == 2)
					res.TypeB.Add(item);
				if (item.Category == 3)
					res.TypeC.Add(item);
				if (item.Category == 4)
					res.TypeD.Add(item);
				if (item.Category == 5)
					res.TypeK.Add(item);
				if (item.Category == 6)
					res.TypeL.Add(item);
				if (item.Category == 7)
					res.TypeO.Add(item);
				if (item.Category == 8)
					res.TypeP.Add(item);
				if (item.Category == 9)
					res.TypeS.Add(item);
				if (item.Category == 10)
					res.TypeT.Add(item);
				if (item.Category == 11)
					res.TypeU.Add(item);
				if (item.Category == 12)
					res.TypeV.Add(item);
				if (item.Category == 13)
					res.TypeW.Add(item);
				if (item.Category == 14)
					res.TypeX.Add(item);
                if (item.Category == 101) res.TypeA.Add(item);
                if (item.Category == 102) res.TypeB.Add(item);
                if (item.Category == 103) res.TypeC.Add(item);
                if (item.Category == 104) res.TypeD.Add(item);
                if (item.Category == 105) res.TypeE.Add(item);
                if (item.Category == 106) res.TypeF.Add(item);
                if (item.Category == 109) res.TypeO.Add(item);
                if (item.Category == 110) res.TypeP.Add(item);
                if (item.Category == 112) res.TypeS.Add(item);
                if (item.Category == 113) res.TypeT.Add(item);
                if (item.Category == 111) res.TypeL.Add(item);
                if (item.Category == 107) res.TypeM.Add(item);
                if (item.Category == 108) res.TypeN.Add(item);
                if (item.Category == 114) res.TypeQ.Add(item);
                if (item.Category == 201) res.TypeA.Add(item);
                if (item.Category == 202) res.TypeB.Add(item);
                if (item.Category == 203) res.TypeC.Add(item);
                if (item.Category == 204) res.TypeD.Add(item);
                if (item.Category == 205) res.TypeK.Add(item);
                if (item.Category == 206) res.TypeL.Add(item);
                if (item.Category == 207) res.TypeO.Add(item);
                if (item.Category == 208) res.TypeV.Add(item);


            }
            return res;
		}

		public void ClearTargetType(string type)
		{
			webView.ExecuteScriptAsync($"clear{type}()");
		}

		public void ClearLine()
		{
            webView.ExecuteScriptAsync($"clearLine()");
        }

		public void AddTarget()
		{
			// フィールド名から該当のリストを取得
			Type typeOfMyStruct = typeof(TargetListsModel);
			FieldInfo[] fieldInfos = typeOfMyStruct.GetFields();
			var propInfo = typeof(TargetListsModel).GetProperties();

			foreach (var info in propInfo)
			{
				// 帯状道路施設（終点）は非表示
				//if (info.Name.Equals("TypeP"))
				//	continue;

				// NOTE: 解析範囲は表示対象外（念のため）
				if (info.Name.Equals("TypeS") || info.Name.Equals("TypeT"))
					continue;

				// 一旦、非表示にする
				ClearTargetType(info.Name);

				// 種別の表示をする準備
				var prop = typeof(TargetListsModel).GetProperty(info.Name);
				var items = prop.GetValue(targetLists);

				foreach (var item in (List<SignItem>)items)
				{
					if (item.TimeTarget.Lat == 0 || item.TimeTarget.Lng == 0)
						continue;

					if (Mode.Equals(PairsMode.Lot))
					{
						webView.ExecuteScriptAsync($"drop{info.Name}(" +
							$"\"{ item.TimeTarget.Lat.ToString(Define.LatLngFormat)}\"," +
							$"\"{item.TimeTarget.Lng.ToString(Define.LatLngFormat)}\"," +
							$"\"{item.TimeTarget.Time.ToString(Define.DateTimeFormat)}\"," +
							$"\"{item.No}\"," +
							$"\"{item.address.Sonota}\")");
					}
                    else if (Mode.Equals(PairsMode.Label))
                    {
						webView.ExecuteScriptAsync($"drop{info.Name}(" +
							$"\"{item.TimeTarget.Lat.ToString(Define.LatLngFormat)}\"," +
							$"\"{item.TimeTarget.Lng.ToString(Define.LatLngFormat)}\"," +
							$"\"{item.TimeTarget.Time.ToString(Define.DateTimeFormat)}\"," +
							$"\"{item.Label}\"," +
                            $"\"{item.address.Sonota}\")");
                    }
				}
			}
		}

		public void AddTargets(List<SignItem> items, Color color, int border)
		{
			targetLists = AllocateTargets(items);

			// アイコン追加
			AddTarget();

			// 帯状追加
			DrawLine(color, border);
		}

		// line

		public void AddLine(Color color, int border)
		{
			// 帯状追加
			DrawLine(color, border);
		}

		public void DrawLine(Color color, int border)
        {
			if (targetLists == null)
				return;

			if (targetLists.TypeM.Count == 0 || targetLists.TypeN.Count == 0)
			{
				ClearTargetType("TypeM");
				ClearTargetType("TypeN");

				webView.ExecuteScriptAsync($"clearLine()");

				return;
			}

			webView.ExecuteScriptAsync($"clearLine()");

			foreach (var dst in targetLists.TypeN)
            {
				if (dst.LongItem == null)
					continue;

				if (string.IsNullOrEmpty(dst.LongItem.SrcNoStr))
					continue;

				var src = targetLists.TypeM.Where(x => x.No.Equals(dst.LongItem.SrcNoStr)).FirstOrDefault();
				if (src == null)
					continue;

				// 念のため一旦カラーオブジェクトを作っておく
				Color c = Color.FromArgb(color.R, color.G, color.B);

				// 16進数RGBカラーコードを取得
				var color16 = ColorTranslator.ToHtml(c);

                //開始、中間地点、終了を線で結ぶ
                double srcLat = 0;
                double srcLng = 0;
                double dstLat = 0;
                double dstLng = 0;
                //中間地点分繰り返し
                for (int i = 0; i < src.LongItemMiddlePoint.Count; i++)
                {
                    //最初は開始から中間まで
                    if (i == 0)
                    {
                        srcLat = src.TimeTarget.Lat;
                        srcLng = src.TimeTarget.Lng;
                        dstLat = src.LongItemMiddlePoint[i].Lat;
                        dstLng = src.LongItemMiddlePoint[i].Lng;
                    }
                    else
                    {
                        //最初以外は中間を結ぶ
                        srcLat = src.LongItemMiddlePoint[i - 1].Lat;
                        srcLng = src.LongItemMiddlePoint[i - 1].Lng;
                        dstLat = src.LongItemMiddlePoint[i].Lat;
                        dstLng = src.LongItemMiddlePoint[i].Lng;
                    }

                    //線描画
                    webView.ExecuteScriptAsync($"drawLine(" +
                        $"\"{srcLat.ToString(Define.LatLngFormat)}\"," +
                        $"\"{srcLng.ToString(Define.LatLngFormat)}\"," +
                        $"\"{dstLat.ToString(Define.LatLngFormat)}\"," +
                        $"\"{dstLng.ToString(Define.LatLngFormat)}\"," +
                        $"\"{color16}\"," +
                        $"\"{border.ToString()}\")");

                }

                //中間があれば
                if (src.LongItemMiddlePoint.Count > 0)
                {
                    //中間の最後と終了を結ぶ
                    srcLat = src.LongItemMiddlePoint.Last().Lat;
                    srcLng = src.LongItemMiddlePoint.Last().Lng;
                }
                else
                {
                    //開始と終了を結ぶ
                    srcLat = src.TimeTarget.Lat;
                    srcLng = src.TimeTarget.Lng;
                }

                //終了を結ぶ
                dstLat = dst.TimeTarget.Lat;
                dstLng = dst.TimeTarget.Lng;
                webView.ExecuteScriptAsync($"drawLine(" +
                        $"\"{srcLat.ToString(Define.LatLngFormat)}\"," +
                        $"\"{srcLng.ToString(Define.LatLngFormat)}\"," +
                        $"\"{dstLat.ToString(Define.LatLngFormat)}\"," +
                        $"\"{dstLng.ToString(Define.LatLngFormat)}\"," +
                        $"\"{color16}\"," +
                        $"\"{border.ToString()}\")");
            }
        }

        /// <summary>
        /// KML情報をマップへ登録
        /// </summary>
        /// <param name="kmls"></param>
        /// <param name="index"></param>
        public void AddKmls(List<KmlModel> kmls, int index)
        {
            // Kmlから表示用Pin生成
            List<Pin> kmlPinList = new List<Pin>();
            foreach (var kml in kmls)
            {
                if (kml.IsValidLatLng)
                {
                    kmlPinList.Add(Pin.CreateByKmlModel(kml));
                }
            }
            if (kmlPinList.Count == 0) return;

            // KMLアイコン一旦削除して追加
            ClearKmls();
            foreach (Pin pin in kmlPinList)
            {
                AddKml(pin);
            }

            webView.Parent.Invoke((Action)(() =>
            {
                // 表示調整
                webView.Select();
            }));

            if (index == -1) return;

            // カーソル表示しなおして、Map中央を表示
            MoveCursor(kmlPinList[index]);
            MoveMap(kmlPinList[index]);
        }


        public void ClearKmls()
        {
            webView.ExecuteScriptAsync("clearKmls()");
        }

        /// <summary>
        /// マップ上にカーソルアイコンを描画
        /// </summary>
        /// <param name="pin"></param>
        public void MoveCursor(Pin pin)
        {
            ClearCursor();
            webView.ExecuteScriptAsync($"dropCursorPin(\"{pin.Lat}\", \"{pin.Lng}\", \"{pin.When}\")");
        }

        public void ClearCursor()
        {
            webView.ExecuteScriptAsync("clearCursor()");
        }

        private void AddKml(Pin pin)
        {
            //kmlファイル名もjsに渡す（カーソル選択時の動画切替のため）
            string ss = System.Web.HttpUtility.JavaScriptStringEncode(pin.movieFile);
            webView.ExecuteScriptAsync($"dropKmlPin(\"{pin.Lat}\", \"{pin.Lng}\", \"{pin.When}\", \"{pin.Direction}\", \"{pin.IsGoogleType}\", \" {ss}\")");

        }

        public void MoveMap(SignItem item)
        {
			webView.ExecuteScriptAsync($"moveMap(\"{item.TimeTarget.Lat.ToString(Define.LatLngFormat)}\", \"{item.TimeTarget.Lng.ToString(Define.LatLngFormat)}\")");
		}

        public void MoveMap(Pin pin)
        {

            webView.ExecuteScriptAsync($"moveMap(\"{pin.Lat}\", \"{pin.Lng}\")");
        }
        public void OpenPopup(SignItem item)
        {
			webView.ExecuteScriptAsync($"openPopup(\"{item.TimeTarget.Lat.ToString(Define.LatLngFormat)} \", \" {item.TimeTarget.Lng.ToString(Define.LatLngFormat)}\")");
		}

		public void CloseAllPopup()
		{
			webView.ExecuteScriptAsync($"closePopup()");
		}

		public void SetMaxBounds()
		{
            webView.ExecuteScriptAsync($"SetMaxBounds()");
        }

        public void AddPolygon(string pos)
        {
            webView.ExecuteScriptAsync($"addPolygon(\"{pos}\",\"red\")");
        }

        public void ClearPolygon()
        {
            webView.ExecuteScriptAsync($"clearPolygonLayer()");
        }

        public void SetZoom(int i)
        {
            webView.ExecuteScriptAsync($"setZoomManual(\"{i}\")");
        }
        public void dropCursorPin(string lat, string lng)
        {
            webView.ExecuteScriptAsync("clearCursor()");
            webView.ExecuteScriptAsync($"dropCursorPin(\"{lat}\", \"{lng}\")");
        }
    }

	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	public class JsToCs
	{
		// アイコンダブルクリックイベント
		public delegate void JsIconDClickEventHandler(string serialNo);
		public event JsIconDClickEventHandler JsIconDClickEvent;

		public delegate void JsGetMapBoundsEventHandler(string bounds);
		public event JsGetMapBoundsEventHandler JsGetMapBoundsEvent;

        public delegate void JsIconClickEventHandler(string serialNo);
        public event JsIconDClickEventHandler JsIconClickEvent;

        public delegate void JsExcelClickEventHandler(string serialNo);
        public event JsExcelClickEventHandler JsExcelClickEvent;

        public delegate void JsKmlClickEventHandler(string datetimeStr, string kmlFile);
        public event JsKmlClickEventHandler JsKmlClickEvent;

        public void IconClick(string serialNo)
		{
			JsIconClickEvent(serialNo);
		}

        public void IconDClick(string serialNo)
		{
			// アイコンダブルクリックイベント
			JsIconDClickEvent(serialNo);
		}

		public void GetMapBounds(string bounds)
		{
			JsGetMapBoundsEvent(bounds);
		}

		public void OnExcelClick(string serialNo)
		{
			JsExcelClickEvent(serialNo);

        }

        /// <summary>
        /// KMLアイコンクリック時イベント
        /// </summary>
        /// <param name="datetimeStr"></param>
        /// <param name="kmlFile">kmlファイル</param>
        public void OnKmlClick(string datetimeStr, string kmlFile)
        {
            if (JsKmlClickEvent is null) return;

            JsKmlClickEvent(datetimeStr, kmlFile);
        }
    }
}
