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
using Newtonsoft.Json.Linq;
using Service.WebApi.Catalog.Services;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IRefreshToken _refreshToken;
        private readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;

        public LoginController(IUserManager userManager, IRefreshToken refreshToken, dbRmTools_Context context, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _refreshToken = refreshToken;
            _context = context;
            _tokenManager = tokenManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authentication ([FromBody] Login req)
        {
            ServiceResponseSingle<LoginRes> res = new ServiceResponseSingle<LoginRes>();
            try
            {
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
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();

            }
            return new OkObjectResult(res);
        }

        [HttpGet]
        public IActionResult RefreshToken()
        {
            ServiceResponseSingle<LoginRes> res = new ServiceResponseSingle<LoginRes>();
            try
            {
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
                    res.Message = "Refresh token success";
                    res.Data = logRes;

                }
                else
                {
                    res.Code = 1;
                    res.Message = "Refresh token not found";
                }
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();

            }

            return new OkObjectResult(res);

        }

        [Authorize]
        [DisableCors]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            ServiceResponseSingle<LoginRes> res = new ServiceResponseSingle<LoginRes>();
            try
            {
                var cookies = Request.Cookies;
                string token = cookies["Token"]; //refresh token untuk dapatkan token

                if (token != null)
                {
                    var principals = _tokenManager.GetPrincipal(token);
                    if (principals.status == true)
                    {
                        //Nonactive isLoggin
                        var data = await _context.TblUsers.Where(x => x.Id == principals.Id).FirstOrDefaultAsync();

                        data.IsLogin = false;
                        data.LastLogin = DateTime.Now;
                        _context.TblUsers.Update(data);
                        await _context.SaveChangesAsync();

                        var _ = await _context.TblJwtRepositories.Where(x=> x.UserId == principals.Id).ToListAsync();
                        foreach(var item in _)
                        {
                            item.IsStop = true;
                            item.EndTime = DateTime.Now;
                            _context.TblJwtRepositories.Update(item);
                            await _context.SaveChangesAsync();
                        }

                        // Hapus cookie
                        Response.Cookies.Delete("RefreshToken");
                        Response.Cookies.Delete("Token");


                        res.Code = 1;
                        res.Message = "Logout succesfully";

                    }
                    else
                    {
                        res.Code = -1;
                        res.Message = "Token is not valid";
                    }

                }
                else
                {
                    res.Code = 1;
                    res.Message = "Please login to access this resource";
                }
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();

            }
  
           

            return new OkObjectResult(res);

        }
    }
}
