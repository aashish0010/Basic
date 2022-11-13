using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace Basic.Application.Service
{
    public class DashBoardService : IDashBoardService
    {

        public Uservalidate GetUserClaimsData(string token)
        {
            Uservalidate user = new Uservalidate();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = NormalFunctions.Decrypt(tokenS.Claims.First(claim => claim.Type == "name").Value);
            return user;
        }
    }
}
