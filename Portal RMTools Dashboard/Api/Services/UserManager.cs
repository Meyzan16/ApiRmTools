using Api.Components;
using Api.Models.SQLServer;
using Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Services
{
    public interface IUserManager
    {
        (string jwt, string error) Authenticate(string username, string password);

    }
    public class UserManager : IUserManager
    {

        private readonly CredentialAttr _appSettings;
        private readonly dbRmTools_Context _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;

        public UserManager(IOptions<CredentialAttr> appSettings, IConfiguration config, dbRmTools_Context context,
            IHttpContextAccessor accessor)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _configuration = config;
            _accessor = accessor;
        }
        public (string jwt, string error) Authenticate(string username, string password)
        {
            //var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);
            string errormsg = "";
            string jwt = "";
            string refresh_token = "";
            bool LoginAllowed = false;
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            string host = _accessor.HttpContext.Request.Host.Value;
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
                    return (null, errormsg);
                }
            }
            else
            {
                errormsg = "Username tidak terdaftar";
            }


            if (LoginAllowed)
            {
                //generate keyUID (belum dipakai)
                byte[] keyBytes = new byte[16];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(keyBytes);
                }
                string keyUID = Convert.ToBase64String(keyBytes);
                //end generate


                // authentication successful so generate jwt token claims
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                                 new Claim("UID", EncryptString(data.Id.ToString())),
                    }),

                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(GetConfig.AppSetting["TokenLifetime"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Issuer
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                jwt = tokenHandler.WriteToken(token);

                data.StartLogin = DateTime.Now;
                data.Token = jwt;
                data.Uid = EncryptString(data.Id.ToString());
                data.SecretKey = keyUID;
                _context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }

            return (jwt, errormsg);
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
