using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; }
        public long? SizeVideoId { get; set; }
        public string SizeVideoName { get; set; }
        public byte Status { get; set; }
    }
}
