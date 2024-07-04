using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class HistoryAction : BaseEntity
    {
        public long ProjectId { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
    }
}
