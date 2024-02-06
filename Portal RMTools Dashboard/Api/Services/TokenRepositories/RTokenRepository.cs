
using Api.Models.SQLServer;
using Api.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Api.Repositories
{
    public class RTokenRepository : IRTokenRepository
    {
        private IConfiguration _configuration;

        public RTokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool AddToken(TblJwtRepository token)
        {
            using (dbRmTools_Context db = new dbRmTools_Context(_configuration))
            {
                db.TblJwtRepositories.Add(token);

                return db.SaveChanges() > 0;
            }
        }

        public bool ExpireToken(TblJwtRepository token)
        {
            using (dbRmTools_Context db = new dbRmTools_Context(_configuration))
            {
                db.TblJwtRepositories.Update(token);

                return db.SaveChanges() > 0;
            }
        }

        public TblJwtRepository GetToken(string refresh_token, int userId, string ipAddress)
        {
            using (dbRmTools_Context db = new dbRmTools_Context(_configuration))
            {
                return db.TblJwtRepositories.FirstOrDefault(x =>  x.RefreshToken == refresh_token  && x.UserId == userId && x.ClientIp == ipAddress);
            }
        }
    }
}
