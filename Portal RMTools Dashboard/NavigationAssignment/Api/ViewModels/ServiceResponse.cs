using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public class AccessNavigateResponse
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public int Order { get; set; }
        public string? IconClass { get; set; }
    }
}
