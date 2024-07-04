using ToolExportVideo.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolExportVideo.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string InfoMessage { get; set; }
        public Response()
        {
            Success = true;
        }
        public void SetSuccess(object data)
        {
            this.Success = true;
            this.Data = data;
        }
        public void SetError(ErrorCode errorCode, string errorMessage = "", object data = null)
        {
            this.Success = false;
            this.ErrorMessage = errorMessage;
            this.ErrorCode = errorCode;
            this.Data = data;
        }
    }
}
