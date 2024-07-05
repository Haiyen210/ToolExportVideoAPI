using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolExportVideo.DL;
using ToolExportVideo.Models;

namespace ToolExportVideo.BL
{
    public class BLAccount : BLBase
    {
        public DLAccount _dlAccount { get; set; }
        public BLAccount()
        {
            _dlAccount = new DLAccount();
        }
        public Account? Login(LoginParam login)
        {
            return _dlAccount.Login(login);
        }
    }
}
