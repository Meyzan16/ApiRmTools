
using Api.Models.SQLServer;

namespace Api.Repositories
{
    public interface IRTokenRepository
    {
        bool AddToken(TblJwtRepository token);

        bool ExpireToken(TblJwtRepository token);

        TblJwtRepository GetToken(string refresh_token, int userId, string ipAddress);
    }
}
