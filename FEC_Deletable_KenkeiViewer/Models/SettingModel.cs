using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Deletable_KenkeiViewer.Models
{
	public class SettingModel
	{
		public OffsetItem Offset { get; set; } = new OffsetItem();
	}

	public class OffsetItem
	{
		public int Selected { get; set; } = 0;
		public float Left { get; set; } = 0;
		public float Right { get; set; } = 0;
		public bool Manual { get; set; } = false;
	}
}
