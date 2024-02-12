using Api.ViewModels;
using Api.Models.SQLServer;
using Api.Services;
using Api.Tools;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Transactions;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Repositories
{
    public interface IMasterKelolaanRepo
    {
        Task<(bool status, string error, List<ResponseResultFile> data)> UploadFile(UploadFile_VM model);   

    }

    public class MasterKelolaan : IMasterKelolaanRepo
    {
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        public MasterKelolaan(ITokenManager tokenManager,dbRmTools_Context context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }
        #region UploadFile
        public async Task<(bool status, string error, List<ResponseResultFile> data)> UploadFile(UploadFile_VM model)
        {
            try
            {
                if (model == null)
                {
                    return (false, "Request not found", new List<ResponseResultFile>());
                }

                List<ResponseResultFile> data = new List<ResponseResultFile>();

                ResponseResultFile test = new ResponseResultFile()
                {
                    DataSuccess = 20
                };
                data.Add(test);
               


                return (true, "", data);
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new List<ResponseResultFile>());
            }
        }
        #endregion

       
    }
}
