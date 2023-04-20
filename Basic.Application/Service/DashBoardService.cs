using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Dapper;
using Basic.Infrastracture.Entity;
using Dapper;
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
        public dynamic GetUserDetail(string token)
        {
            var dash = new DashBoard();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            dash.UserName = NormalFunctions.Decrypt(jwt.Claims.First(c => c.Type == "unique_name").Value);
            string sql = "select * from tbl_user where username=@username and isactive <> 'n'";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", dash.UserName);
            var data = DbHelper.RunQueryDynamicallywithoutasync(sql, param, "n");
            List<string> dataList = new List<string>();
            string[] Bio = data.FirstOrDefault().bio.Split(',');
            foreach (var items in Bio)
            {
                dataList.Add(items);
            }

            return new
            {
                UserName = data.FirstOrDefault()?.username,
                Email = data.FirstOrDefault()?.email,
                FirstName = data.FirstOrDefault()?.firstname,
                MiddleName = data.FirstOrDefault()?.middlename,
                LastName = data.FirstOrDefault()?.lastname,
                PhoneNumber = data.FirstOrDefault()?.phonenum,
                DateOfBirth = data.FirstOrDefault()?.dateofbirth,
                Gender = data.FirstOrDefault()?.gender,
                Bio = dataList,
                Images = GetImage(data.FirstOrDefault()?.username)

            };
        }
        public dynamic GetImage(string username, string flag = null)
        {
            var lst = new List<string>();
            var imglst = new object();
            if (flag == "y")
                imglst = _context.UserDoc.Where(x => x.UserName == username && x.IsActive == "y" && x.IsProfilePic == "y").Select(x => new { x.ImageUrl, x.ImageCategory, x.IsProfilePic, x.IsActive }).ToList();
            else
                imglst = _context.UserDoc.Where(x => x.UserName == username && x.IsActive == "y").Select(x => new { x.ImageUrl, x.ImageCategory, x.IsProfilePic, x.IsActive }).ToList();

            return imglst;
        }


        public dynamic GetReceiverDetail(string username)
        {
            string sql = "select * from tbl_user where username=@username and isactive <> 'n'";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", username);
            var data = DbHelper.RunQueryDynamicallywithoutasync(sql, param, "n");
            List<string> dataList = new List<string>();
            if (!string.IsNullOrEmpty(data.FirstOrDefault().bio))
            {
                string[] Bio = data.FirstOrDefault().bio.Split(',');
                foreach (var items in Bio)
                {
                    dataList.Add(items);
                }
            }



            return new
            {
                UserName = data.FirstOrDefault()?.username,
                Email = data.FirstOrDefault()?.email,
                FirstName = data.FirstOrDefault()?.firstname,
                MiddleName = data.FirstOrDefault()?.middlename,
                LastName = data.FirstOrDefault()?.lastname,
                PhoneNumber = data.FirstOrDefault()?.phonenum,
                DateOfBirth = data.FirstOrDefault()?.dateofbirth,
                Gender = data.FirstOrDefault()?.gender,
                Bio = dataList,
                Images = GetImage(data.FirstOrDefault()?.username)

            };

            //return _context.tbl_user.Where(x => x.UserName == username).FirstOrDefault();
        }
    }
}
