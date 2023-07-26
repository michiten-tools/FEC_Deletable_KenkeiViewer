using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Models
{
    class NoteModel
    {
        List<NoteItem> Items { get; set; } = new List<NoteItem>();

        public DateTime UpdateDate { get; set; }
        public Version Version { get; set; }

    }

    public class NoteItem
    {
        public string No { get; set; }
        public string Note { get; set; }
    }
}
