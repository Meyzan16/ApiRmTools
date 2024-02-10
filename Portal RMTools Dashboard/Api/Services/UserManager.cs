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
            string? StageMode = GetConfig.AppSetting["env"];

            string errormsg = "";
            string jwt = "";
            string refresh_token = "";
            bool LoginAllowed = false;

            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            string hostName = _accessor.HttpContext.Request.Host.Value;

            var data = _context.TblUsers.Where(x => x.Username == username).FirstOrDefault();

            if (data != null)
            {
                if (data.IsLogin != true || data.IsLogin == null)
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
                    errormsg = "Account is active";
                }
            }
            else
            {
                errormsg = "Username not registered";
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
                    Expires = SetTokenExpiration(StageMode, "token"), // 1m
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
                data.IsLogin = true;
                data.StartLogin = DateTime.Now;
                data.Token = jwt;
                data.Uid = encryptedUid;
                _context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                //add cookies
                // Return JWT and refresh token as cookies
                _cookies.SetTokenCookies("Token", jwt, SetTokenExpiration(StageMode, "token"));
                _cookies.SetTokenCookies("RefreshToken", refresh_token, SetTokenExpiration(StageMode, "refreshtoken"));
                //end cookies
            }

            return (jwt, errormsg, refresh_token);
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

        private DateTime SetTokenExpiration(string stageMode, string tokenType)
        {
            double tokenLifeTime = double.Parse(GetConfig.AppSetting["TokenLifetime"]);
            double refreshLifeTime = double.Parse(GetConfig.AppSetting["RefreshLifetime"]);

            double lifetimeInDays;
            if (tokenType.ToLower() == "refreshtoken")
            {
                return DateTime.UtcNow.AddDays(refreshLifeTime);
            }
            else
            {
                // Token lifetime based on stage mode
                if (stageMode.ToLower() == "development" && tokenType.ToLower() == "token")
                {
                    return DateTime.UtcNow.AddDays(tokenLifeTime);
                }
                else
                {
                    return DateTime.UtcNow.AddMinutes(tokenLifeTime);
                }
            }
        }
    }
}
