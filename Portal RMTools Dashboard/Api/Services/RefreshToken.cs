using Api.Repositories;
using Api.Models.SQLServer;
using Api.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Service.WebApi.Catalog.Services;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Api.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server;

namespace Api.Authentication
{
    public interface IRefreshToken
    {
      
        (string token, string refreshToken, string error) DoRefreshToken(string refresh_tokenn);
    }
    public class RefreshToken : IRefreshToken
    {
        //repository to handler the database  
        private IRTokenRepository _repo;

        private readonly CredentialAttr _appSettings;

        private readonly ITokenManager _tokenManager;

        private readonly IConfiguration _configuration;

        private readonly dbRmTools_Context _context;

        private readonly ICookies _cookies;

        public RefreshToken(IRTokenRepository repo, IOptions<CredentialAttr> appSettings, ITokenManager tokenManager, dbRmTools_Context context,
            IConfiguration configuration, ICookies cookies)
        {
            _repo = repo;
            _appSettings = appSettings.Value;
            _tokenManager = tokenManager;
            _configuration = configuration;
            _context = context;
            _cookies = cookies;
        }

        //get the access_token by refresh_token  
        public (string token, string refreshToken, string error) DoRefreshToken(string refresh_tokenn)
        {
            var jwt = "";
            var jwttoken = _context.TblJwtRepositories.Where(x => x.RefreshToken == refresh_tokenn).FirstOrDefault();

            if (jwttoken != null)
            {
                var principals = _tokenManager.GetPrincipal(jwttoken.Token);

                if (principals != null)
                {
                    //ambil jwt saat ini
                    var tokenRepo = _repo.GetToken(refresh_tokenn, principals.Id, principals.IpAddress);

                    // decison token 
                    if (tokenRepo == null)
                    {
                        return ("", "", "Silahkan login");
                    }

                    if (tokenRepo.IsStop == true)
                    {
                        return ("", "", "Refresh token tidak berlaku");
                    }

                    // get claims from existing token
                    if (principals.status == true)
                    {
                        // expire the old refresh_token 
                        tokenRepo.EndTime = DateTime.Now;
                        tokenRepo.IsStop = true;
                        var updateFlag = _repo.ExpireToken(tokenRepo);
                        //end

                        // update expries refresh token yang tidak digunakan lagi
                        using (dbRmTools_Context db = new dbRmTools_Context(_configuration))
                        {
                            foreach (var a in db.TblJwtRepositories.Where(x => x.UserId == principals.Id && x.IsStop == false).ToList())
                            {
                                if (a != null)
                                {
                                    a.EndTime = DateTime.Now;
                                    a.IsStop = true;
                                    db.TblJwtRepositories.Update(a);
                                    db.SaveChanges();
                                }
                            }
                        }

                        if (updateFlag)
                        {

                            // authentication successful so generate jwt token claims
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                            string encryptedUid = EncryptString(principals.Id.ToString());
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                     new Claim("UID", encryptedUid),
                                     new Claim("IpAddress",  principals.IpAddress),
                                     new Claim("Hostname",  principals.HostName),
                                }),

                                Expires = DateTime.UtcNow.AddHours(double.Parse(GetConfig.AppSetting["RefreshLifetime"])), //1 jam
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                                Issuer = _appSettings.Issuer
                            };

                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            jwt = tokenHandler.WriteToken(token);

                            // add a new refresh_token  
                            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
                            var addFlag = _repo.AddToken(new TblJwtRepository
                            {
                                UserId = principals.Id,
                                ClientIp = principals.IpAddress,
                                RefreshToken = refresh_token,
                                Token = jwt,
                                StartTime = DateTime.Now,
                                Hostname = principals.HostName,
                                IsStop = false
                            });

                            var data = _context.TblUsers.Where(x => x.Id == principals.Id).FirstOrDefault();
                            //update data lama
                            data.StartLogin = DateTime.Now;
                            data.Token = jwt;
                            data.Uid = encryptedUid;
                            _context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();

                            //add cookies
                            _cookies.SetTokenCookies("Token", jwt, DateTime.UtcNow.AddMinutes(double.Parse(GetConfig.AppSetting["TokenLifetime"])));
                            _cookies.SetTokenCookies("RefreshToken", refresh_token, DateTime.UtcNow.AddHours(double.Parse(GetConfig.AppSetting["RefreshLifetime"])));
                            //end cookies
                            return (jwt, refresh_token, "");
                        }
                        else
                        {
                            return ("", "", "Not created new token from refresh token");
                        }

                    }
                    else
                    {
                        return ("", "", principals.message);
                    }
                }
                else
                {
                    return ("", "", "Token not valid and not accesss");
                }
            }
            else
            {
                return ("", "", "Please login for access this resource");
            }
           

           

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
