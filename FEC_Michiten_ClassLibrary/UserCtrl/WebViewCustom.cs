using FEC_Michiten_ClassLibrary.Map;
using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Util;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FEC_Michiten_ClassLibrary.Util.Define;

namespace FEC_Michiten_ClassLibrary.UserCtrl
{
    public partial class WebViewCustom : UserControl
    {

        public MapFunc miniMapFunc;

        public delegate void NavigationCompletEventHandler();
        public event NavigationCompletEventHandler NavigationCompEvent;

        public event MapFunc.DClickEventHandler WebViewDClickEvent;
        public event MapFunc.IconClickEventHandler WebViewIconClickEvent;
        public event MapFunc.ExcelClickEventHandler WebViewExcelClickEvent;
        public event MapFunc.MapBoundsEventHandler WebViewMapBoundsEvent;
        public event MapFunc.KmlIconClickEventHandler WebViewMapKmlClickEvent;

        public WebViewCustom()
        {
            InitializeComponent();
        }

        private async void WebViewCustom_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                webView.CreationProperties = new CoreWebView2CreationProperties();
                webView.CreationProperties.BrowserExecutableFolder = Define.WebViewRuntimePath;
                await InitializeAsync();
            }
        }


        private async Task InitializeAsync()
        {
            var options = new CoreWebView2EnvironmentOptions("--allow-file-access-from-files");
            var environment = await CoreWebView2Environment.CreateAsync(Define.WebViewRuntimePath, null, options);
            await webView.EnsureCoreWebView2Async(environment);

        }



        private void webView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            try
            {
                Log.Info("webView_CoreWebView2InitializationCompleted " + Define.WebMapPath);
                webView.CoreWebView2.Navigate(Define.WebMapPath);
            }
            catch(Exception ex)
            {
                Log.Error("webView_CoreWebView2InitializationCompleted", ex);
                throw ex;
            }
        }

        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            webView.ExecuteScriptAsync("setZoomMain()");
            miniMapFunc = new MapFunc(webView);

            miniMapFunc.KmlIconClickEvent += WebViewMapKmlClickEvent;
            miniMapFunc.DClickEvent += WebViewDClickEvent;
            miniMapFunc.IconClickEvent += WebViewIconClickEvent;
            miniMapFunc.ExcelClickEvent += WebViewExcelClickEvent;
            miniMapFunc.MapBoundsEvent+= WebViewMapBoundsEvent;

            if (NavigationCompEvent != null)
                NavigationCompEvent();
        }

        public void AddIconTarget(List<SignItem> items)
        {

            miniMapFunc.AddTargets(items, Color.Red, 4);

        }

        public void MoveMap(SignItem item)
        {
            miniMapFunc.MoveMap(item);
        }

        public void MoveMap(Pin item)
        {
            miniMapFunc.MoveMap(item);
        }

        public void AddKmls(List<KmlModel> kmls, int index)
        {
            miniMapFunc.AddKmls(kmls, index);
        }

        public void SetCursor(Pin pin)
        {
            miniMapFunc.dropCursorPin(pin.Lat, pin.Lng);
        }

        public void SetCursor(SignItem item)
        {
            miniMapFunc.dropCursorPin(item.TimeTarget.Lat.ToString(), item.TimeTarget.Lng.ToString());
        }

        public void OpenPopup(SignItem item)
        {
            miniMapFunc.OpenPopup(item);
        }

        public void DrawLine(SignItem src, SignItem dst)
        {
            double srcLat = 0;
            double srcLng = 0;
            double dstLat = 0;
            double dstLng = 0;

            srcLat = src.TimeTarget.Lat;
            srcLng = src.TimeTarget.Lng;
            dstLat = dst.TimeTarget.Lat;
            dstLng = dst.TimeTarget.Lng;

            var color16 = ColorTranslator.ToHtml(Color.Blue);
            int border = 5;

            webView.ExecuteScriptAsync($"drawLine(" +
                        $"\"{srcLat.ToString(Define.LatLngFormat)}\"," +
                        $"\"{srcLng.ToString(Define.LatLngFormat)}\"," +
                        $"\"{dstLat.ToString(Define.LatLngFormat)}\"," +
                        $"\"{dstLng.ToString(Define.LatLngFormat)}\"," +
                        $"\"{color16}\"," +
                        $"\"{border.ToString()}\")");

        }
    }
}
