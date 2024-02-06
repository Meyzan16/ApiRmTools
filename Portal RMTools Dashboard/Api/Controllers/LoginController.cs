using Api.Models.SQLServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Api.Services;
using Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Api.Components;
using Api.Authentication;
using NuGet.Common;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IRefreshToken _refreshToken;

        public LoginController(IUserManager userManager, IRefreshToken refreshToken)
        {
            _userManager = userManager;
            _refreshToken = refreshToken;
        }

        [AllowAnonymous]
        [DisableCors]
        [HttpPost]
        public IActionResult Authentication ([FromBody] Login req)
        {
            ServiceResponseSingle<LoginRes> res = new ServiceResponseSingle<LoginRes>();

            var Token = _userManager.Authenticate(req.username, req.password);

            if (Token.jwt == "" || Token.jwt == null)
            {
                res.Code = -1;
                res.Message = Token.error;
                return new OkObjectResult(res);
            }

            var logRes = new LoginRes()
            {
                token = Token.jwt,
                refreshToken = Token.refreshToken
            };

            res.Code = 1;
            res.Message = "success";
            res.Data = logRes;

            return new OkObjectResult(res);
        }

        [AllowAnonymous]
        [DisableCors]
        [HttpGet]
        public IActionResult RefreshToken()
        {
            ServiceResponseSingle<LoginRes> res = new ServiceResponseSingle<LoginRes>();
            var cookies = Request.Cookies;
            string refreshToken = cookies["RefreshToken"];
            if(refreshToken != null)
            {
                var Token = _refreshToken.DoRefreshToken(refreshToken);

                if (Token.token == "" || Token.token == null)
                {
                    res.Code = -1;
                    res.Message = Token.error;
                    return new OkObjectResult(res);
                }

                var logRes = new LoginRes()
                {
                    token = Token.token,
                    refreshToken = Token.refreshToken
                };

                res.Code = 1;
                res.Message = "success";
                res.Data = logRes;

            }
            else
            {
                res.Code = 1;
                res.Message = "Refresh token not found";
            }

            return new OkObjectResult(res);

        }


    }
}
