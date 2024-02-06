using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class DataTableReq_VM
    {
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Parent { get; set; }
    }
}
