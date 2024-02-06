using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class CredentialAttr
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
    }

    public class DecryptUID
    {
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public string HostName { get; set; }

        public bool status { get; set; }

        public string message { get; set; }
    }

    //public class CookieHeaderValue
    //{
    //    public string Name { get; set; }
    //    public string Value { get; set; }
    //    public DateTimeOffset? Expires { get; set; }
    //    public string Domain { get; set; }
    //    public string Path { get; set; }
    //    public bool HttpOnly { get; set; }
    //    public bool Secure { get; set; }
    //    public SameSiteMode SameSite { get; set; }

    //}
}
