using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class NavigationRes_VM
    {
        public Int64 Number { get; set; }
        public int Id { get; set; }
        public string Nama { get; set; }
        public int Type { get; set; }
        public string Route { get; set; }
        public int? OrderBy { get; set; }
        public int IsVisible { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public string Visible_Name { get; set; }
        public string Tipe { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public string DeletedTime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }    
    }
}
