using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Dapper;
using Basic.Infrastracture.Entity;
using Dapper;

namespace Basic.Application.Service
{
    public class ForgetPasswordService : IForgetPasswordService
    {
        private readonly ApplicationDbContext _context;
        public ForgetPasswordService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CommonResponseOpt> GenerateForgetProcessid(string email)
        {
            var userdata = _context.tbl_user.Where(x => x.Email == email).FirstOrDefault();
            if (userdata == null)
            {
                return new CommonResponseOpt()
                {
                    Code = 400,
                    Message = "Email Is Not Register"
                };
            }
            var usercount = _context.OtpManager.Where(x => x.IsValid == "y" && x.IsVerified == null).Count();

            if (usercount > 1)
            {
                var data = _context.OtpManager.Where(x => x.Email == userdata.Email && x.IsValid == "y" && x.IsVerified == null);
                foreach (var items in data)
                {
                    items.IsValid = "n";
                    _context.OtpManager.Update(items);
                    _context.SaveChanges();
                }
            }


            var Otpstring = NormalFunctions.RandomString(7);

            var processid = NormalFunctions.encrypt(userdata.Email);

            await _context.EmailRequest.AddAsync(new ForgetPassword()
            {
                Userid = userdata.Userid,
                Email = email,
                Processid = processid,
                Status = null,
                Createdate = DateTime.UtcNow

            });
            await _context.SaveChangesAsync();
            await _context.OtpManager.AddAsync(new OtpManager()
            {
                Email = email,
                ProcessId = processid,
                OtpCode = Otpstring,
                IsValid = "y",
                IsVerified = null,
                CreatedDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return new CommonResponseOpt()
            {
                Code = 200,
                Message = "Email Send Successfully",
                ProcessId = processid,
                Otp = Otpstring,
                Username = userdata.UserName
            };

        }

        public CommonResponse VerifyUser(string email, string proccessid, string otp)
        {
            var data = _context.OtpManager.Where(x => x.Email == email && x.ProcessId == proccessid && x.IsValid == "y"
            && x.OtpCode == otp);
            if (data.Count() > 0)
            {
                DateTime verifydate = Convert.ToDateTime(data.FirstOrDefault().CreatedDate).AddMinutes(20);
                if (verifydate < DateTime.UtcNow)
                {
                    return new CommonResponse()
                    {
                        Code = 400,
                        Message = "Token Expired"
                    };
                }
                else
                {
                    data.FirstOrDefault().IsVerified = "y";
                    data.FirstOrDefault().VerifiedDate = DateTime.UtcNow;

                    _context.OtpManager.Update(data.FirstOrDefault());
                    _context.SaveChanges();

                    return new CommonResponse()
                    {
                        Code = 200,
                        Message = "Password Changed Succefullly"
                    };

                }


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


        public void ChangePassword(string email, string password)
        {
            string sql = " update tbl_user set password=pwdencrypt(@password),loginattempt=null,islocked='n' where email=@email";
            DynamicParameters param = new DynamicParameters();
            param.Add("@email", email);
            param.Add("@password", password);
            DbHelper.Execute(sql, "n", param);

        }
    }
}
