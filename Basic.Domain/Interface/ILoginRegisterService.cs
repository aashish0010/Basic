using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface ILoginRegisterService
    {
        Task<IEnumerable<Uservalidate>> Login(LoginRequest login);
        Task<IEnumerable<CommonResponse>> Register(Register Register);
    }
}
