using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class Account:BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte? Role { get; set; }
        public byte? Status { get; set; }
    }
}
