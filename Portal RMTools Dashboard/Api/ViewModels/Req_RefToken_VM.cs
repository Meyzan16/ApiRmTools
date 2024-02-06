using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class Req_RefToken_VM
    {
        public string refresh_token { get; set; }
        public string jwttoken { get; set; }
        public string ipAddress { get; set; }
    }
}
