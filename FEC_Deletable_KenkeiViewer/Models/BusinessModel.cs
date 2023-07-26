using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Deletable_KenkeiViewer.Models
{
	class BusinessModel
	{
		public string BusinessName { get; set; }
		public string RootDir { get; set; }
		public List<WorkModel> Works { get; set; } = new List<WorkModel>();
		public DateTime UpdateDate { get; set; }
		public DateTime ResistDate { get; set; }
		public Version Version { get; set; }
	}
}
