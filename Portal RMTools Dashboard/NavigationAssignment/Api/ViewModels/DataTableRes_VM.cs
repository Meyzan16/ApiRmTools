using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class DataTableRes_VM
    {
        public Int64 Number { get; set; }
        public int Id { get; set; }
        public string NamaMenu { get; set; }
        public string NamaRole { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public string DeletedTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public string ActiveByRole { get; set; }
        public string IsDeleteByMenu { get; set; }


    }
}
