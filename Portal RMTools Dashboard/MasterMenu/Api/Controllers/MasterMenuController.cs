using Api.Components;
using Api.Models.SQLServer;
using Api.Repositories;
using Api.Services;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterMenuController : ControllerBase
    {
        private readonly IDecryptManager _decryptManager;
        public readonly ITokenManager _tokenManager;
        public readonly IMasterMenuRepo _news_Repo;
        private readonly dbRmTools_Context _context;

        public MasterMenuController(IDecryptManager decryptManager, dbRmTools_Context context, ITokenManager tokenManager, IMasterMenuRepo news_Repo)
        {
            _decryptManager = decryptManager;
            _context = context;
            _tokenManager = tokenManager;
            _news_Repo = news_Repo;
        }

        #region CREATE
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TblMasterNavigation req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<TblMasterNavigation>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _news_Repo.CreateAsync(req);

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

        #region DELETE
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] IdArray_VM req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<string>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _news_Repo.DeleteAsync(req.ids);

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
        [HttpPost]
        public async Task<IActionResult> View([FromBody] Id_VM req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<TblMasterNavigation>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _news_Repo.ViewAsync(req.Id);

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

        #region UPDATE
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] TblMasterNavigation req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();

            var res = new ServiceResponseSingle<string>();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _news_Repo.UpdateAsync(req);

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
                //return new OkObjectResult(res);
            }


            return new OkObjectResult(res);

        }
        #endregion

        #region LOADDATA
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] DataTableReq_VM req)
        {
            var res = new ServiceResponseSingle<ServiceResponseDataTable<NavigationRes_VM>>();
            try
            {
                var DecryptUID = await _tokenManager.GetPrincipal();
                if (DecryptUID.status == true)
                {

                    var _ = await _news_Repo.LoadDataAsync(req.sortColumn, req.sortColumnDir, req.pageNumber, req.pageSize, req.Name, req.Type, req.Parent);

                    if (_.status == true)
                    {
                        var data = new ServiceResponseDataTable<NavigationRes_VM>()
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
