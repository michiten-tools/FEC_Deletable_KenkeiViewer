using FEC_Michiten_ClassLibrary.Models;
using FEC_Michiten_ClassLibrary.Properties;
using FEC_Michiten_ClassLibrary.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FEC_Michiten_ClassLibrary.Util.Define;

namespace FEC_Michiten_ClassLibrary.Pairs
{
	public class ListFunc
	{
		DataGridView view;
		string rootDir;

        public ListFunc(DataGridView _view, string _rootDir = "")
		{
			view = _view;
			rootDir = _rootDir;
		}

		public void UpdateList(List<SignItem> items, PairsMode mode)
		{
			// リストをクリア
			view.Rows.Clear();

			// リストを追加
			foreach (var item in items)
			{
				// 帯状道路施設（終点）は非表示
				if (item.Category == 8)
					continue;

				view.Rows.Add();

				int index = view.Rows.Count - 1;
				view.Rows[index].Cells["no"].Value = item.No;
				//view.Rows[index].Cells["viewLabel"].Value = item.Label;
				view.Rows[index].Cells["category"].Value = CategoryDefine.GetCategoryStrWithoutMark(item.Category);

				if (item.TimeTarget != null)
				{
					if (item.TimeTarget.Lat != 0 || item.TimeTarget.Lng != 0)
					{
						view.Rows[index].Cells["time"].Value = item.TimeTarget.Time.ToString(Define.DateTimeFormat);
						view.Rows[index].Cells["lon"].Value = item.TimeTarget.Lng.ToString(Define.LatLngFormat);
						view.Rows[index].Cells["lat"].Value = item.TimeTarget.Lat.ToString(Define.LatLngFormat);
					}
				}

				//エクセルファイルがあれば画像を表示
				if (ExistsExcel(rootDir, item.No))
					view.Rows[index].Cells["viewExcel"].Value = new Bitmap(Resources.xlsx);
				else
					view.Rows[index].Cells["viewExcel"].Value = new Bitmap(Resources.xlsxNon);

                view.Rows[index].Cells["label"].Value = item.Label;
                //if (item.IsSumi)
                //	view.Rows[index].DefaultCellStyle.BackColor = Color.Gray;
            }

			// 列表示の切り替え
			if (mode.Equals(PairsMode.Lot))
			{
				view.Columns["no"].Visible = true;
				//view.Columns["viewLabel"].Visible = false;
			}
			else if (mode.Equals(PairsMode.Label))
            {
				view.Columns["no"].Visible = false;
				//view.Columns["viewLabel"].Visible = true;
			}
		}

		private bool ExistsExcel(string dir, string serialNo)
		{
			if (string.IsNullOrEmpty(dir))
				return false;

			string excelFile = Path.Combine(dir, Define.DirExcel, $"{serialNo}.xlsx");

			if (File.Exists(excelFile))
				return true;
			else
				return false;
		}

		public int GetItemIndex(List<SignItem> items, int rowIndex)
        {
			var serialNo = view.Rows[rowIndex].Cells["no"].Value.ToString();
			if (string.IsNullOrEmpty(serialNo))
				return -1;

			var item = items.Where(x => x.No.Equals(serialNo)).FirstOrDefault();
			if (item == null)
				return -1;

			return items.IndexOf(item);
        }

		public SignItem GetSelectedItem(List<SignItem> items, int rowIndex)
		{
			var serialNo = view.Rows[rowIndex].Cells["no"].Value?.ToString();
			if (string.IsNullOrEmpty(serialNo))
				return null;

			var item = items.Where(x => x.No.Equals(serialNo)).FirstOrDefault();
			return item;
		}

		public SignItem GetSelectedItem(List<SignItem> items)
        {
			if (view.SelectedRows.Count == 0)
				return null;

			string no = view.SelectedRows[0].Cells["no"].Value?.ToString();
			var res = items.Where(x => x.No.Equals(no)).FirstOrDefault();

			if (res == null)
				return null;
			else
				return res;

        }

		public SignItem GetSelectedItem(List<SignItem> items, string serialNo)
        {
			if (items.Count == 0)
				return null;

			return items.Where(x => x.No.Equals(serialNo)).FirstOrDefault();
        }

		public int GetRowIndex(string serialNo)
		{
			foreach (DataGridViewRow row in view.Rows)
			{
				string no = row.Cells["no"].Value.ToString();

				if (serialNo.Equals(no))
					return row.Index;
			}

			return -1;
		}

		/// <summary>
		/// 選択してた行までスクロール
		/// </summary>
		/// <param name="view"></param>
		public void Scroll()
		{
			if (view.SelectedRows.Count != 0)
				view.FirstDisplayedScrollingRowIndex = view.SelectedRows[0].Index;
		}

		public int GetRowIndex(List<SignItem> items, string serialNo)
		{
			return items.FindIndex(f => f.No.Equals(serialNo));
		}

    }
}
