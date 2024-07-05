using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ToolExportVideo.BL;
using ToolExportVideo.Common;
using ToolExportVideo.Library;
using ToolExportVideo.Models;
using ToolExportVideo.Respones;

namespace NTH.WOW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginParam login)
        {
            var response = new Response();
            try
            {
                if (login != null && !string.IsNullOrWhiteSpace(login.UserName) && !string.IsNullOrWhiteSpace(login.Password))
                {
                    var blAccount = new BLAccount();
                    var account = blAccount.Login(login);
                    if (account != null)
                    {
                        var token = AuthozirationUtility.RenderAccessToken(account);
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            LoginRespone res = new LoginRespone() { Token = $"{JwtBearerDefaults.AuthenticationScheme} {token}", User = account };
                            response.SetSuccess(res);
                        }
                    }
                    else
                    {
                        response.SetError(ErrorCode.EmployeeNotFound, "Không tìm thấy tài khoản");
                    }
                }
                else
                {
                    response.SetError(ErrorCode.InvalidParam, "Tham số không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                response.SetError(ErrorCode.Unknown, ex.ToString());
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(List<Account> register)
        {
            var response = new Response();
            try
            {
                var blAccount = new BLAccount();
                var employee = blAccount.SaveData(register);
            }
            catch (Exception ex)
            {
                response.SetError(ErrorCode.Unknown, ex.ToString());
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
