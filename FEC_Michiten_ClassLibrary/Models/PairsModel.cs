using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Models
{
	public class PairsModel
	{
		public string PairsRoot { get; set; }
		public List<SignItem> Items { get; set; } = new List<SignItem>();
		public Version Version { get; set; }
		public DateTime PairsUpdateDate { get; set; }
	}
}
