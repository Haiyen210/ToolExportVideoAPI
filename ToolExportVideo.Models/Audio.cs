using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class Audio : BaseEntity
    {
        public long ProjectId { get; set; }
        public string AudioMp3 { get; set; }
        public byte Status { get; set; }
    }
}
