using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolExportVideo.BL;
using ToolExportVideo.DL;
using ToolExportVideo.Library;
using ToolExportVideo.Models;

namespace ToolExportVideo.API
{
    [Authorize]
    public class BaseController<TEntity> : ControllerBase
    {
        public BLBase    _blBase { get; set; }
        public BaseController(BLBase blBase)
        {
            _blBase = blBase;
        }
        [HttpPost("SaveData")]
        public async  Task<IActionResult> SaveData(List<TEntity> datas)
        {
            var response = new Response();
            try
            {
                response.Data = _blBase.SaveData(datas);
            }
            catch (Exception ex)
            {
                response.SetError(ErrorCode.Unknown, ex.ToString());
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
        [HttpGet("SelectAll")]
        public async Task<IActionResult> SelectAll()
        {
            var response = new Response();
            try
            {
                response.SetSuccess(_blBase.SelectAll<TEntity>());
            }
            catch (Exception ex)
            {
                response.SetError(ErrorCode.Unknown, ex.ToString());
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("selectnewcode")]
        public async Task<IActionResult> SelectNewCode()
        {
            var response = new Response();
            try
            {
                response.SetSuccess(_blBase.SelectNewCode<string>(this.GetType().Name.Replace("Controller", "")));
            }
            catch (Exception ex)
            {
                response.SetError(ErrorCode.Unknown, ex.ToString());
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
        
    }
}
