using Newtonsoft.Json;
using System;

namespace FEC_Deletable_KenkeiViewer.Models
{
    public class RenameModel
    {
        public bool Detect { get; set; } = false;
        public bool Selected { get; set; } = false;

        public Guid RoadId { get; set; } = Guid.Empty;
        public Guid RectId { get; set; } = Guid.Empty;

        public string Rename { get; set; } = string.Empty;

    }
}
