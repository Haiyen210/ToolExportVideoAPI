
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToolExportVideo.Library;
using ToolExportVideo.Models;
using static System.Net.WebRequestMethods;

namespace ToolExportVideo.Common
{
    public class AuthozirationUtility 
    {
        private static Microsoft.AspNetCore.Http.HttpContext _context;

        public static string RenderAccessToken(Account access_user)
        {
            var jwtToken = new JwtSecurityToken(
                claims: GetTokenClaims(access_user),
                expires: DateTime.Now.AddHours(24),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.SecretSercurityKey)), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }


        public JwtSecurityToken GetRequestAccessToken(AuthorizationHandlerContext context)
        {
            try
            {
                var token = GetToken(context);
                token = token.Replace("Bearer ", "");
                return new JwtSecurityTokenHandler().ReadJwtToken(token);
            }
            catch
            {
                return null;
            }
        }

        private string GetToken(AuthorizationHandlerContext context)
        {
            var httpContext = ((Microsoft.AspNetCore.Mvc.ActionContext)context.Resource).HttpContext;
            var token = httpContext.Request.Headers["Authorization"].ToString();

            return string.IsNullOrWhiteSpace(token) ? string.Empty : token;
        }
        public static void SetHttpContext(Microsoft.AspNetCore.Http.HttpContext context)
        {
            _context = context;
        }
        public static T GetClaim<T>(string claimName)
        {
            try
            {
                var identity = _context.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    //IEnumerable<Claim> claims = identity.Claims;
                    // or
                    var value = identity.FindFirst(claimName)?.Value;
                    return Commonfunc.ConvertToType<T>(value);
                }
            }
            catch (Exception)
            {

            }
            return Commonfunc.ConvertToType<T>(null);
        }
        private static IEnumerable<Claim> GetTokenClaims(Account access_user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, access_user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, access_user.FullName),
                new Claim(JwtRegisteredClaimNames.Sid, access_user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, access_user.Id.ToString()),
                new Claim(Constant.mscRoles, access_user.Role.ToString()),
                new Claim(Constant.mscAppCode, ConfigUtil.GetAppSettings<string>(AppSettingKeys.AppCode)),
            };
        }

    }
}
