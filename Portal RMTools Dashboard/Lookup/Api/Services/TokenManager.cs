using Api.Components;
using Api.Models.SQLServer;
using Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Services
{
    public interface ITokenManager
    {
        Task<(bool status, string error, TblUser data)> GetPrincipal();

    }

    public class TokenManager : ITokenManager
    {
        private IHttpContextAccessor _context;
        private readonly IDecryptManager _decryptManager;
        private readonly dbRmTools_Context _contextData;

        public TokenManager(dbRmTools_Context dbContextSqlServer,IHttpContextAccessor httpContextAccessor, IDecryptManager decryptManager)
        {
            _context = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _decryptManager = decryptManager;
            _contextData = dbContextSqlServer;
        }
        public async Task<(bool status, string error, TblUser data)> GetPrincipal()
        {
            try
            {
                string authorization = _context.HttpContext.Request.Headers["Authorization"];
                if (authorization != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = authorization.Split(" ")[1];
                    var parsedToken = tokenHandler.ReadJwtToken(token);

                    var UID = parsedToken.Claims.Where(c => c.Type == "UID").FirstOrDefault().Value;

                    var decryptedUID = _decryptManager.Decrypt(UID);

                    if (decryptedUID.status == true)
                    {
                        var data = await _contextData.TblUsers.Where(x => x.Id == decryptedUID.Id).FirstOrDefaultAsync();

                        return (true, "", data);

                    }
                    else
                    {
                        return (false, "User data decryption not found", new TblUser());
                    }
                }

                else
                {
                    return (false, "Access is not permitted", new TblUser());
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message.ToString(), new TblUser());
            }


        }
    }
}
