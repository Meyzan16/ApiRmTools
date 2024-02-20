using Api.Components;
using Api.Models.SQLServer;
using Api.Repositories;
using Api.Services;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterKelolaanController : ControllerBase
    {
        private readonly IDecryptManager _decryptManager;
        public readonly ITokenManager _tokenManager;
        public readonly IMasterKelolaanRepo _executeRepo;
        private readonly dbRmTools_Context _context;

        public MasterKelolaanController(IDecryptManager decryptManager, dbRmTools_Context context, ITokenManager tokenManager, IMasterKelolaanRepo executeRepo)
        {
            _decryptManager = decryptManager;
            _context = context;
            _tokenManager = tokenManager;
            _executeRepo = executeRepo;
        }

        #region Upload File
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadFile_VM model)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();
            var res = new ServiceResponseSingle<ServiceResponse<ResponseResultFile>>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.UploadFile(model);

                    if (_.status == true)
                    {

                        res.Code = 1;
                        res.Message = "success";

                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = _.error;
                    }

                }
                else
                {
                    res.Code = -2;
                    res.Message = DecryptUID.error;
                }
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();

            }

            return new OkObjectResult(res);

        }
        #endregion


    }
}
