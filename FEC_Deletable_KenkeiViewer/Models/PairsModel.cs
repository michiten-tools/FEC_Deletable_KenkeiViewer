using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FEC_Deletable_KenkeiViewer.Models
{
    public enum BusinessMode { Michiten, Kenkei, Tetsuten }

    public class PairsModel
	{
		public string PairsRoot { get; set; }
		public List<SignItem> Items { get; set; } = new List<SignItem>();
		public List<RoadModel> Roads { get; set; } = new List<RoadModel>();
		public Version Version { get; set; }
		public DateTime PairsUpdateDate { get; set; }

		public BusinessMode BusinessMode { get; set; }

		/// <summary>
		/// 指定ItemのAnnotation更新
		/// </summary>
		/// <param name="itemIdx"></param>
		/// <param name="newAnnotation"></param>
		/// <returns></returns>
		public bool UpdateAnnotation(int itemIdx, Annotation newAnnotation)
        {
			if (itemIdx >= Items.Count)
            {
				return false;
            }

			Items[itemIdx].Annotation = newAnnotation;

			return true;
        }


		/// <summary>
		/// 指定したItemNoのItemインスタンスを返す
		/// 該当ない場合はnull
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		public SignItem GetSignItemByItemNo(string itemNo)
        {
			return Items.Where(x => x.No == itemNo).FirstOrDefault();
        }


		/// <summary>
		/// 保持しているItems内から指定SignNoのIndex位置を返す
		/// 該当ない場合は-1を返す
		/// </summary>
		/// <param name="itemNo"></param>
		/// <returns></returns>
		public int GetSignItemIndexByItemNo(string itemNo)
        {
			if (itemNo is null) return -1;

			var idx = 0;
            foreach (var item in Items)
            {
				if (item.No == itemNo) return idx;
				idx++;
            }

			return -1;
        }


		/// <summary>
		/// 指定したIndexのオフセット情報を更新
		/// </summary>
		/// <param name="targetIdx"></param>
		/// <param name="newSignItem"></param>
		/// <returns></returns>
		public bool UpdateOffsetInfo(int targetIdx, SignItem newSignItem)
        {
			var targetItem = Items[targetIdx];
			targetItem.Offset = newSignItem.Offset;
			targetItem.OrgTarget = newSignItem.OrgTarget;
			targetItem.TimeTarget = newSignItem.TimeTarget;

			return true;
        }


		/// <summary>
		/// 指定したIndexのソースファイル名をフルパスで返す
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		public string GetSourceFileFullPath(int idx)
        {
			return Path.Combine(PairsRoot, Items[idx].SourceFile); ;
        }


		


	}
}
