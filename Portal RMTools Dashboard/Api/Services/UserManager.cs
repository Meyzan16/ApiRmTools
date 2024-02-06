using Api.Components;
using Api.Models.SQLServer;
using Api.Repositories;
using Api.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Net.Http.Headers;
using Azure;
using NuGet.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Api.Services
{
    public interface IUserManager
    {
        (string jwt, string error, string refreshToken) Authenticate(string username, string password);

    }
    public class UserManager : IUserManager
    {

        private readonly CredentialAttr _appSettings;
        private readonly dbRmTools_Context _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly IRTokenRepository _repo;
        private readonly ICookies _cookies;

        public UserManager(IOptions<CredentialAttr> appSettings, IConfiguration config, dbRmTools_Context context,
            IHttpContextAccessor accessor, IRTokenRepository repo, ICookies cookies)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _configuration = config;
            _accessor = accessor;
            _repo = repo;
            _cookies = cookies;

        }
        public (string jwt, string error, string refreshToken) Authenticate(string username, string password)
        {
            string errormsg = "";
            string jwt = "";
            string refresh_token = "";
            bool LoginAllowed = false;

            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            string hostName = _accessor.HttpContext.Request.Host.Value;

            var data = _context.TblUsers.Where(x => x.Username == username).FirstOrDefault();

            if (data != null)
            {
                try
                {
                    if (password == GetConfig.AppSetting["AppSettings:GlobalSettings:GlobalPassword"])
                    {
                        LoginAllowed = true;
                    }
                    else if (data.IsActive == false)
                    {
                        errormsg = "User not active";
                    }
                    else if (password == data.Password)
                    {
                        LoginAllowed = true;
                    }
                    else
                    {
                        errormsg = "Password Invalid";
                    }
                }
                catch (Exception ex)
                {
                    errormsg = ex.ToString() == null ? ex.InnerException.Message : ex.ToString() + "!!->" + ex.Message;
                    return (null, errormsg, null);
                }
            }
            else
            {
                errormsg = "Username tidak terdaftar";
            }

            if (LoginAllowed)
            {
                refresh_token = Guid.NewGuid().ToString().Replace("-", "");

                // authentication successful so generate jwt token claims
                var tokenHandler = new JwtSecurityTokenHandler();
                string encryptedUid = EncryptString(data.Id.ToString());
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                                     new Claim("UID", encryptedUid),
                                     new Claim("IpAddress",  heserver.AddressList[1].ToString()),
                                     new Claim("Hostname",  heserver.HostName),

                    }),

                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(GetConfig.AppSetting["TokenLifetime"])), //1 menit
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Issuer
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                jwt = tokenHandler.WriteToken(token);

                //add token
                var rToken = new TblJwtRepository
                {
                    UserId = data.Id,
                    ClientIp = heserver.AddressList.Length > 0 ? heserver.AddressList[1].ToString() : null,
                    RefreshToken = refresh_token,
                    Token = jwt,
                    Hostname = heserver.HostName,
                    StartTime = DateTime.Now,
                    IsStop = false
                };
                _repo.AddToken(rToken);
                //end token

                data.StartLogin = DateTime.Now;
                data.Token = jwt;
                data.Uid = encryptedUid;
                _context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                //add cookies
                // Return JWT and refresh token as cookies
                _cookies.SetTokenCookies("Token", jwt, DateTime.UtcNow.AddMinutes(double.Parse(GetConfig.AppSetting["TokenLifetime"])));
                _cookies.SetTokenCookies("RefreshToken", refresh_token, DateTime.UtcNow.AddDays(double.Parse(GetConfig.AppSetting["RefreshLifetime"])));
                //end cookies


            }

            return (jwt, errormsg, refresh_token);
        }


        private void SetTokenCookies(string cookieName, string token, DateTime expires)
        {
            // Set cookie options (customize based on your requirements)
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                Expires = expires
            };

            if (GetConfig.AppSetting["env"] == "Production")
            {
                cookieOptions.Secure = true;
            }

            _accessor.HttpContext.Response.Cookies.Append(cookieName, token, cookieOptions);
        }

        static string EncryptString(string Id)
        {
            using (Aes aesAlg = Aes.Create())
            {
                var secretKey = GetConfig.AppSetting["AppSettings:GlobalSettings:SecretKey"];

                aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(Id);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }


        }



    }
}
