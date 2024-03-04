using Api.Models.SQLServer;
using Api.Services;
using Api.Tools;
using Api.ViewModels;
using MasterRole_GetById.Tools;

namespace Api.Repositories
{
    public interface IMasterKelolaanRepo
    {
        Task<(bool status, string error)> UploadFile(UploadFile_VM model);

    }

    public class MasterKelolaan : IMasterKelolaanRepo
    {
        public readonly ITokenManager _tokenManager;
        private readonly dbRmTools_Context _context;
        public MasterKelolaan(ITokenManager tokenManager, dbRmTools_Context context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }
        #region UploadFile
        public async Task<(bool status, string error)> UploadFile(UploadFile_VM model)
        {
            try
            {
                if (model == null)
                {
                    return (false, "Request not found");
                }

                var baseDirectory = _context.TblMasterParameters.Where(x => x.Key == "DirFile").Select(x => x.Value).FirstOrDefault();
                string AllowedFileExt = GetConfig.AppSetting["AllowedFileExt"]; //ext is allowed
                var FileExt = Path.GetExtension(model.ExcelFile.FileName.Replace(" ", "_")).ToLower();
                double fileSizeInMB = (double)model.ExcelFile.Length / (1024 * 1024); //konversi byte to MB
                double allowedFileSize = int.Parse(GetConfig.AppSetting["AllowedFileSize"]) * 1024 * 1024; //konversi to MB
                double fileSizeInKB = 0;

                if (!AllowedFileExt.Contains(FileExt))
                {
                    return (false, "Extension not allowed");
                }
                else if (fileSizeInMB > allowedFileSize)
                {
                    return (false, "Maximum file size is 2 MB");
                }
                else
                {
                    var filename = Path.GetFileName(model.ExcelFile.FileName.Replace(" ", "_"));
                    DateTime AppDateInt = DateTime.Now;
                    var randomString = RandomAlphanumeric.RandomString(4);
                    string NameFileNotExt = Path.GetFileNameWithoutExtension(model.ExcelFile.FileName.Replace(" ", "_"));
                    fileSizeInKB = (double)model.ExcelFile.Length / 1024;
                    var targetfolder = baseDirectory + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month;
                    //decision create  folder
                    if (!Directory.Exists(targetfolder))
                    {
                        Directory.CreateDirectory(targetfolder);
                    }
                    string fileName = $"{NameFileNotExt}_{DateTime.Now:ddMMyyyy}_{randomString}{FileExt}";
                    var fullpath = Path.Combine(targetfolder, fileName);
                    using (var stream = new FileStream(fullpath, FileMode.Create))
                    {
                        await model.ExcelFile.CopyToAsync(stream);
                    };

                    TblMasterFile test = new TblMasterFile()
                    {
                        FileName = filename,
                        FileSize = $"{fileSizeInKB.ToString():N2} KB",
                        Path = baseDirectory,
                        FullPath = fullpath,
                        Ext = FileExt,
                        DataSuccess = 20,
                        DataFailed = 0,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedById = _tokenManager.GetPrincipal().Result.data.Id
                    };
                    await _context.TblMasterFiles.AddAsync(test);
                    await _context.SaveChangesAsync();


                }

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString());
            }
        }
        #endregion


    }
}
