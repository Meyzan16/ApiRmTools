using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class LoginRes
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string Error { get; set;  }
    }

}
