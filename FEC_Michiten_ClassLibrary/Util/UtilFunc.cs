using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FEC_Michiten_ClassLibrary.Util.Define;

namespace FEC_Michiten_ClassLibrary.Util
{
	public class UtilFunc
	{
		const int ERROR_SHARING_VIOLATION = 32;
		const int ERROR_LOCK_VIOLATION = 33;

		public static void ErrMsg(string str)
        {
            MessageBox.Show(str,
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

		public static List<Control> GetAllControls(Control top)
		{
			List<Control> res = new List<Control>();
			foreach (Control c in top.Controls)
			{
				res.Add(c);
				res.AddRange(GetAllControls(c));
			}
			return res;
		}

		private static bool IsFileLocked(Exception exception)
		{
			int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
			return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
		}

		public static bool CanReadFile(string filePath)
		{
			try
			{
				using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
				{
					if (fileStream != null) fileStream.Close();
				}
			}
			catch (IOException ex)
			{
				if (IsFileLocked(ex))
				{
					return false;
				}
			}
			finally
			{ }
			return true;
		}

		public static Size GetJpegFrameSize(string jpeg)
		{
			if (!File.Exists(jpeg))
				return new Size(0, 0);

			var shellAppType = Type.GetTypeFromProgID("Shell.Application");
			dynamic shell = Activator.CreateInstance(shellAppType);

			Shell32.Folder objFolder = shell.NameSpace(Path.GetDirectoryName(jpeg));
			Shell32.FolderItem folderItem = objFolder.ParseName(Path.GetFileName(jpeg));

			string val = objFolder.GetDetailsOf(folderItem, 176);

			int w = 0;
			if (int.TryParse(Regex.Replace(val, @"[^0-9]", ""), out w))
			{
				val = objFolder.GetDetailsOf(folderItem, 178);
				int h = 0;
				if (int.TryParse(Regex.Replace(val, @"[^0-9]", ""), out h))
					return new Size(w, h);
			}
			return new Size(0, 0);
		}

		/// <summary>
		/// https://www.ipentec.com/document/csharp-resize-image
		/// </summary>
		/// <param name="size"></param>
		/// <param name="img"></param>
		/// <returns></returns>
		public static Bitmap GetResizeBitmap(Size size, string img)
		{
			try
			{
				using (Bitmap bmp = new Bitmap(img))
				{
					Bitmap res = new Bitmap(size.Width, size.Height);
					Graphics g = Graphics.FromImage(res);

					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.DrawImage(bmp, 0, 0, size.Width, size.Height);
					g.Dispose();

					return res;
				}
			}
			catch
			{
				return null;
			}
		}

		public static bool CheckFrameComment(string imgFile)
		{
			// コメント取得
			string comment = UtilFunc.GetFileComment(imgFile);

			if (string.IsNullOrEmpty(comment))
			{
				UtilFunc.ErrMsg("申し訳ございません。このファイルは読込み可能なデータではありません。");
				return false;
			}

			if (!comment.StartsWith("FURUKAWA"))
			{
				UtilFunc.ErrMsg("申し訳ございません。このファイルは読込み可能なデータではありません。");
				return false;
			}

			string[] strs = comment.Split(',');
			if (strs.Length != 6)
			{
				UtilFunc.ErrMsg("申し訳ございません。このファイルは読込み可能なデータではありません。");
				return false;
			}

			return true;
		}

		private static string GetFileComment(string file)
		{
			if (!File.Exists(file))
				return "";

			var shellAppType = Type.GetTypeFromProgID("Shell.Application");
			dynamic shell = Activator.CreateInstance(shellAppType);

			Shell32.Folder objFolder = shell.NameSpace(Path.GetDirectoryName(file));
			Shell32.FolderItem folderItem = objFolder.ParseName(Path.GetFileName(file));

			return objFolder.GetDetailsOf(folderItem, (int)FileProperty.Comment);
		}
	}
}
