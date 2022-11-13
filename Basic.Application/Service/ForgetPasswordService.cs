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


            await _context.EmailRequest.AddAsync(new ForgetPassword()
            {

                Userid = 123,
                Email = email,
                Processid = NormalFunctions.RandomString(20),
                Status = "failed",
                Createdate = DateTime.UtcNow.ToString(),
                Approvedate = null
            });
            await _context.SaveChangesAsync();
            return new CommonResponse()
            {
                Code = 200,
                Message = "Message Send Successfully"
            };

        }

        public Task<CommonResponse> VerifyUser(string email, string proccessid)
        {
            throw new NotImplementedException();
        }
    }
}
