using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Basic.Application.Service
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IConfiguration _config;

        public DashBoardService(IConfiguration config)
        {
            _config = config;
        }

        public Uservalidate GetUserClaimsData(string token)
        {
            Uservalidate user = new Uservalidate();
            NormalFunctions nf = new NormalFunctions(_config);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = nf.Decrypt(tokenS.Claims.First(claim => claim.Type == "name").Value);
            return user;
        }
    }
}
