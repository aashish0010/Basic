using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;
using System.IdentityModel.Tokens.Jwt;

namespace Basic.Application.Service
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ApplicationDbContext _context;

        public DashBoardService(ApplicationDbContext context)
        {
            _context = context;
        }
        public DashBoard GetUserClaimsData(string token)
        {
            var dash = new DashBoard();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            dash.UserName = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "unique_name").Value);
            dash.Email = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "Email").Value);
            dash.Role = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "Role").Value);
            dash.Isactive = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "Isadmin").Value);
            dash.UserCount = _context.tbl_user.Count();
            return dash;
        }
    }
}
