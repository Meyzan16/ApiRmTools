using Api.Components;
using Api.Models.SQLServer;
using Api.Services;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IDecryptManager _decryptManager;
        public readonly ITokenManager _tokenManager;

        private readonly dbRmTools_Context _context;
        public UtilityController(IConfiguration config, IDecryptManager decryptManager,
            dbRmTools_Context context, ITokenManager tokenManager)
        {
            _decryptManager = decryptManager;
            _context = context;
            _tokenManager = tokenManager;
        }


        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> LoadDataRM()
        {
            return Ok("asa");


        }

    }
}
