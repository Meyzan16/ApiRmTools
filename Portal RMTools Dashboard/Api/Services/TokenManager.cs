using Api.Components;
using Api.Models.SQLServer;
using Api.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Service.WebApi.Catalog.Services
{
    public interface ITokenManager
    {
        DecryptUID GetPrincipal(string token);
    }
    public class TokenManager : ITokenManager
    {
        private IHttpContextAccessor _context;
        private readonly IDecryptManager _decryptManager;
        private readonly dbRmTools_Context _contextData;

        public TokenManager(dbRmTools_Context dbContextSqlServer, IHttpContextAccessor httpContextAccessor, IDecryptManager decryptManager)
        {
            _context = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _decryptManager = decryptManager;
            _contextData = dbContextSqlServer;
        }
        public DecryptUID GetPrincipal(string token)
        {
            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var parsedToken = tokenHandler.ReadJwtToken(token);
                var UID = parsedToken.Claims.Where(c => c.Type == "UID").FirstOrDefault().Value;
                var decryptUID = _decryptManager.Decrypt(UID);

                DecryptUID data = new DecryptUID()
                {
                    Id = decryptUID.Id,
                    IpAddress = parsedToken.Claims.Where(c => c.Type == "IpAddress").FirstOrDefault().Value,
                    HostName = parsedToken.Claims.Where(c => c.Type == "Hostname").FirstOrDefault().Value,
                    status = decryptUID.status,
                    message = decryptUID.message,
                };

                return data;
            }
            else
            {
                return null;
            }
        }
    }
}
