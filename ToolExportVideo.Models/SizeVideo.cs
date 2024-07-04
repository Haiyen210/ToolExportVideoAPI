using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class SizeVideo : BaseEntity
    {
        public string SizeName { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public byte Status { get; set; }
    }
}
