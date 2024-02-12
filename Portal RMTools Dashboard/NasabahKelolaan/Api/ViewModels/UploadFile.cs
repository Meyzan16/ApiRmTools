namespace Api.ViewModels
{
    public class UploadFile_VM
    {
        public IFormFile ExcelFile { get; set; }
    }

    public class ResponseResultFile
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FullPath { get; set; }
        public int DataSuccess { get; set; }
        public int dataFailed { get; set; }
    }
}
