using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class Image : BaseEntity
    {
        public long ProjectId { get; set; }
        public string Text { get; set; }
        public string TranslateText { get; set; }
        public string LinkImage { get; set; }
        public string Status { get; set; }
    }
}
