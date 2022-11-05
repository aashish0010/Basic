using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailMessage email);
    }
}
