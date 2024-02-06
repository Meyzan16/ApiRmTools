using Api.Components;
using Api.Models.SQLOracle;
using Api.Models.SQLServer;
using Api.Repositories;
using Api.Services;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IDecryptManager _decryptManager;
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        private readonly ModelContext _contextOracle;
        public readonly IUtilityRepo _excuteRepo;

        public UtilityController(IConfiguration config, IDecryptManager decryptManager,
            dbRmTools_Context context, ITokenManager tokenManager, ModelContext contextOracle, IUtilityRepo excuteRepo)
        {
            _decryptManager = decryptManager;
            _context = context;
            _tokenManager = tokenManager;
            _contextOracle = contextOracle;
            _excuteRepo = excuteRepo;

        }



        #region DropdownRMTransaksi
        [Authorize]
        [DisableCors]
        [HttpGet]
        public async Task<IActionResult> DropdownRMTransaksi()
        {
            var res = new ServiceResponseSingle<ListDataDropDown<DropdownDataResponse>>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();

                if (DecryptUID.status == true)
                {
                    var _ = await _excuteRepo.LoadDataAsyncRmTransaksi();

                    if (_.status == true)
                    {
                        var result = new ListDataDropDown<DropdownDataResponse>()
                        {
                            items = _.list,
                            total_count = _.resultCount
                        };

                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = result;
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

        #region DropdownRMTransaksiByParamaters
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> DropdownRMTransaksiByParameters([FromBody] UserId_VM req)
        {
            var res = new ServiceResponseSingle<DropdownDataResponse>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();

                if (DecryptUID.status == true)
                {
                    var result = await _excuteRepo.LoadDataByParamAsyncRmTransaksi(req.UserId);

                    if (result.status == true)
                    {
                      
                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = result.data;
                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = result.error;
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

        #region DropdownRM/BAKomersial
        [Authorize]
        [DisableCors]
        [HttpGet]
        public async Task<IActionResult> DropdownRMBAKomersial()
        {
            var res = new ServiceResponseSingle<ListDataDropDown<DropdownDataResponse>>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();

                if (DecryptUID.status == true)
                {
                    var _ = await _excuteRepo.LoadDataAsyncRMBAKomersial();

                    if (_.status == true)
                    {
                        var result = new ListDataDropDown<DropdownDataResponse>()
                        {
                            items = _.list,
                            total_count = _.resultCount
                        };

                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = result;
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

        #region DropdownRMBAKomersialByParamaters
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> DropdownRMBAKomersialByParameters([FromBody] UserId_VM req)
        {
            var res = new ServiceResponseSingle<DropdownDataResponse>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();

                if (DecryptUID.status == true)
                {
                    var result = await _excuteRepo.LoadDataByParamAsyncRMBAKomersial(req.UserId);

                    if (result.status == true)
                    {

                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = result.data;
                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = result.error;
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

        #region DropdownRM/BACorporasi
        [Authorize]
        [DisableCors]
        [HttpGet]
        public async Task<IActionResult> DropdownRMBACorporasi()
        {
            var res = new ServiceResponseSingle<ListDataDropDown<DropdownDataResponse>>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();

                if (DecryptUID.status == true)
                {
                    var _ = await _excuteRepo.LoadDataAsyncRMBACorporasi();

                    if (_.status == true)
                    {
                        var result = new ListDataDropDown<DropdownDataResponse>()
                        {
                            items = _.list,
                            total_count = _.resultCount
                        };

                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = result;
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

        #region DropdownRMBACorporasiByParamaters
        [Authorize]
        [DisableCors]
        [HttpPost]
        public async Task<IActionResult> DropdownRMBACorporasiByParameters([FromBody] UserId_VM req)
        {
            var res = new ServiceResponseSingle<DropdownDataResponse>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();

                if (DecryptUID.status == true)
                {
                    var result = await _excuteRepo.LoadDataByParamAsyncRMBACorporasi(req.UserId);

                    if (result.status == true)
                    {

                        res.Code = 1;
                        res.Message = "sukses";
                        res.Data = result.data;
                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = result.error;
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
