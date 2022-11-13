using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IForgetPasswordService
    {
        Task<CommonResponse> GenerateForgetProcessid(string email);
        Task<CommonResponse> VerifyUser(string email, string proccessid);
    }
}
