namespace Api.ViewModels
{
    public class ServiceResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
    public class ServiceResponseSingle<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ServiceResponseDataTable<T>
    {
        public int recordTotals { get; set; }
        public List<T> Data { get; set; }
    }


}
