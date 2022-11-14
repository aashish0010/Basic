using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;

namespace Basic.Application.Service
{
    public class ForgetPasswordService : IForgetPasswordService
    {
        private readonly ApplicationDbContext _context;
        public ForgetPasswordService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CommonResponse> GenerateForgetProcessid(string email)
        {
            int userid = _context.tbl_user.Where(x => x.Email == email).FirstOrDefault().Userid;
            if (userid == 0)
            {
                return new CommonResponse()
                {
                    Code = 400,
                    Message = "Email Is Not Register"
                };
            }
            var random = NormalFunctions.RandomString(20);

            await _context.EmailRequest.AddAsync(new ForgetPassword()
            {
                Userid = userid,
                Email = email,
                Processid = random,
                Status = null,
                Createdate = DateTime.UtcNow.ToString(),
                Approvedate = null
            });
            await _context.SaveChangesAsync();

            return new CommonResponse()
            {
                Code = 200,
                Message = random.ToString()
            };

        }

        public CommonResponse VerifyUser(string email, string proccessid)
        {
            var data = _context.EmailRequest.Where(x => x.Email == email && proccessid == x.Processid);
            if (data.Count() > 0)
            {
                if (Convert.ToDateTime(data.FirstOrDefault().Createdate).AddMinutes(10) < DateTime.UtcNow)
                {
                    return new CommonResponse()
                    {
                        Code = 400,
                        Message = "Token Expired"
                    };
                }

                return new CommonResponse()
                {
                    Code = 200,
                    Message = "Password Changed Succefullly"
                };
            }
            else
            {
                return new CommonResponse()
                {
                    Code = 400,
                    Message = "Request Invalid"
                };
            }
        }
    }
}
