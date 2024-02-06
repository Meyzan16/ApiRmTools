using Api.Components;
using Api.Models.SQLServer;
using Api.Repositories;
using Api.Services;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class LookupController : ControllerBase
    {
        private readonly IDecryptManager _decryptManager;
        public readonly ITokenManager _tokenManager;
        public readonly IMasterLookupRepo _executeRepo;
        private readonly dbRmTools_Context _context;

        public LookupController(IDecryptManager decryptManager, dbRmTools_Context context, ITokenManager tokenManager, IMasterLookupRepo executeRepo)
        {
            _decryptManager = decryptManager;
            _context = context;
            _tokenManager = tokenManager;
            _executeRepo = executeRepo;
        }

        #region CREATE
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TblMasterLookup req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<TblMasterNavigationAssignment>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.CreateAsync(req);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "sukses";
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

        #region VIEW
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> View([FromBody] Id_VM req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<TblMasterLookup>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.ViewAsync(req.Id);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = (_.data);
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

        #region DELETE
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] IdArray_VM req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<string>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.DeleteAsync(req.ids);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "sukses";
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

        #region UPDATE
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] TblMasterLookup req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<string>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.UpdateAsync(req);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "sukses";
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

        #region LOADDATA
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] DataTableReq_VM req)
        {
            var res = new ServiceResponseSingle<ServiceResponseDataTable<MasterLookupRes_VM>>();
            try
            {
                var _ = await _executeRepo.LoadDataAsync(req.sortColumn, req.sortColumnDir, req.pageNumber, req.pageSize, req.Type, req.Name);

                if (_.status == true)
                {
                    var data = new ServiceResponseDataTable<MasterLookupRes_VM>()
                    {
                        Data = _.data,
                        recordTotals = _.recordTotals
                    };

                    res.Code = 1;
                    res.Message = "sukses";
                    res.Data = data;
                }
                else
                {
                    res.Code = -2;
                    res.Message = _.error;
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
