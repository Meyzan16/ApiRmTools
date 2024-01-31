using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class Req_RefToken_VM
    {
        public string refresh_tokenn { get; set; }
        public string client_id { get; set; }
        public int employee_id { get; set; } 
        public string jwttoken { get; set; }
        public string ipAddress { get; set; }
        public int role_Id { get; set; }
        public string roleId { get; set; }
    }
}
