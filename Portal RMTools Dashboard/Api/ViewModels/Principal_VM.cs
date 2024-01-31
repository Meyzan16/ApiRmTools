using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class Principal_VM
    {
        public string Nama_Pegawai { get; set; }
        public string Nama_Role { get; set; }
        public string Nama_Unit { get; set; }
        public string KodeJabatan { get; set; }
        public string Npp { get; set; }
        public string Pegawai_Id { get; set; }
        public string Role_Id { get; set; }
        public string Role_Nama_Unit { get; set; }
        public string Role_Unit_Id { get; set; }
        public string Status_Role { get; set; }
        public string Unit_Id { get; set; }
        public string User_Id { get; set; }
        public string User_Role_Id { get; set; }
    }
}
