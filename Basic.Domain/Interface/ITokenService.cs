using Basic.Domain.Entity;
using System.Security.Claims;

namespace Basic.Domain.Interface
{
    public interface ITokenService
    {
        Tokens GenerateToken(string userName, string email, string role, string isadmin);
        Tokens GenerateRefreshToken(string userName, string email, string role, string isadmin);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GetDataFromToken(string token);
        string GetSpecificTokenData(string token, string value);
        bool CheckTheTokenTime(string token);
        bool IsTokenValid(string token);

    }
}
