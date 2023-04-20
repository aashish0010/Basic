using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IForgetPasswordService
    {
        Task<CommonResponseOpt> GenerateForgetProcessid(string email);
        CommonResponse VerifyUser(string email, string proccessid, string otp);

        void ChangePassword(string email, string password);
    }
}
