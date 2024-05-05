using Api.Components;
using Api.Models.SQLServer;
using Api.Repositories;
using Api.Services;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Newtonsoft.Json;

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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestCreated req)
        {
            var res = new ServiceResponseSingle<TblMasterNavigationAssignment>();
            var activity = new TblLogActivity();


            //// Validate the request
            //var validationResults = new List<ValidationResult>();
            //var isValid = Validator.TryValidateObject(req, new ValidationContext(req), validationResults, true);
            //if (!isValid)
            //{
            //    var errors = validationResults
            //        .SelectMany(r => r.MemberNames.Select(m => new ValidationError { Type = m, Message = r.ErrorMessage }))
            //        .ToArray();
            //    activity.Message = JsonConvert.SerializeObject(errors);

            //    return new BadRequestObjectResult(res);
            //}

            //execute request from client
            var DecryptUID = await _tokenManager.GetPrincipal();
            try
            {
                if (DecryptUID.status == true)
                {

                    var _ = await _executeRepo.CreateAsync(req);

                    if (_.status == true)
                    {
                        res.Code = 201;
                        res.Message = "Created successfully";
                        activity.Status = "success";
                        activity.Message = res.Message;
                    }
                    else
                    {
                        res.Code = 400;
                        res.Message = _.error;
                        activity.Status = "error";
                        activity.Message = res.Message;
                    }
                }
                else
                {
                    res.Code = 400;
                    res.Message = DecryptUID.error;
                    activity.Status = "error";
                    activity.Message = res.Message;
                }
            }
            catch (Exception ex)
            {
                res.Code = 500;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();
                activity.Status = "Catch";
                activity.Message = res.Message;

            }

            //private penyimpanan activity
            await LogActivity(nameof(LookupController), nameof(Create), activity.Status, activity.Message, DecryptUID.data.Id);

            HttpContext.Response.ContentType = "application/json";
            return new OkObjectResult(res);

        }
        #endregion

        #region VIEW
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> View([FromBody] Id_VM req)
        {
            var res = new ServiceResponseSingle<TblMasterLookup>();
            var activity = new TblLogActivity();

       


            var DecryptUID = await _tokenManager.GetPrincipal();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.ViewAsync(req.Id);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "Success";
                        res.Data = (_.data);
                        activity.Status = "success";
                        activity.Message = res.Message;
                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = _.error;
                        activity.Status = "error";
                        activity.Message = res.Message;
                    }

                }
                else
                {
                    res.Code = -2;
                    res.Message = DecryptUID.error;
                    activity.Status = "error";
                    activity.Message = res.Message;
                }
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();
                activity.Status = "Catch";
                activity.Message = res.Message;

            }

            //private penyimpanan activity
            await LogActivity(nameof(LookupController), nameof(View), activity.Status, activity.Message, DecryptUID.data.Id);

            HttpContext.Response.ContentType = "application/json";
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
            var activity = new TblLogActivity();

            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.DeleteAsync(req.ids);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "Deleted successfully";
                        activity.Status = "success";
                        activity.Message = res.Message;
                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = _.error;
                        activity.Status = "error";
                        activity.Message = res.Message;
                    }

                }
                else
                {
                    res.Code = -2;
                    res.Message = DecryptUID.error;
                    activity.Status = "error";
                    activity.Message = res.Message;
                }
            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();
                activity.Status = "Catch";
                activity.Message = res.Message;

            }

            //private penyimpanan activity
            await LogActivity(nameof(LookupController), nameof(Delete), activity.Status, activity.Message, DecryptUID.data.Id);

            HttpContext.Response.ContentType = "application/json";
            return new OkObjectResult(res);

        }
        #endregion

        #region UPDATE
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] TblMasterLookup req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();
            var res = new ServiceResponseSingle<string>();
            var activity = new TblLogActivity();
            try
            {
                if (DecryptUID.status == true)
                {
                    var _ = await _executeRepo.UpdateAsync(req);

                    if (_.status == true)
                    {
                        res.Code = 1;
                        res.Message = "Updated Successfully";
                        activity.Status = "success";
                        activity.Message = res.Message;
                    }
                    else
                    {
                        res.Code = -2;
                        res.Message = _.error;
                        activity.Status = "error";
                        activity.Message = res.Message;
                    }

                }
                else
                {
                    res.Code = -2;
                    res.Message = DecryptUID.error;
                    activity.Status = "error";
                    activity.Message = res.Message;
                }

            }
            catch (Exception ex)
            {
                res.Code = -1;
                res.Message = ex.Message == null ? ex.InnerException.ToString() : ex.Message.ToString();
                activity.Status = "Catch";
                activity.Message = res.Message;
            }

            //private penyimpanan activity
            await LogActivity(nameof(LookupController), nameof(Update), activity.Status, activity.Message, DecryptUID.data.Id);

            HttpContext.Response.ContentType = "application/json";
            return new OkObjectResult(res);

        }
        #endregion

        #region LOADDATA
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] DataTableReq_VM req)
        {
            var DecryptUID = await _tokenManager.GetPrincipal();
            var res = new ServiceResponseSingle<ServiceResponseDataTable<MasterLookupRes_VM>>();
            try
            {
                if (DecryptUID.status == true)
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

            HttpContext.Response.ContentType = "application/json";
            return new OkObjectResult(res);

        }
        #endregion

        private async Task LogActivity(string controllerName, string actionName, string status, string message, int userId)
        {
            // Perform logging logic here, such as saving the activity to the database
            var save = new TblLogActivity
            {
                UserId = userId,
                Path = $"{controllerName}/{actionName}",
                Status = status,
                Message = message,
                ActionTime = DateTime.Now,
                Browser = Request.Headers["User-Agent"]
            };

            // Simpan aktivitas ke database
            await _context.TblLogActivities.AddAsync(save);
            await _context.SaveChangesAsync();

        }

    }

}
