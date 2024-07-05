using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolExportVideo.Models;

namespace ToolExportVideo.DL
{
    public class DLAccount : DLBase
    {
        public DLAccount()
        {
        }
        public Account? Login(LoginParam login)
        {
            return ExecuteReader<Account>("Proc_Login", login).FirstOrDefault();
        }
    }
}
