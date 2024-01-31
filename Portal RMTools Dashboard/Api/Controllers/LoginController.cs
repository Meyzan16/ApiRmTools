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

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IDecryptManager _decryptManager;

        private readonly dbRmTools_Context _context; 
        public LoginController(IConfiguration config, IUserManager userManager, IDecryptManager decryptManager, 
            dbRmTools_Context context)
        {
            _userManager = userManager;
            _decryptManager = decryptManager;
            _context = context;
        }

  
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
                JWT_Token = Token.jwt,
            };


            res.Code = 1;
            res.Message = "success";
            res.Data = logRes;

            return new OkObjectResult(res);

        }
    }
}
