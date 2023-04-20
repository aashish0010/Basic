using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace Basic.Application.Service
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        public EmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendEmail(EmailMessage email)
        {
            try
            {
                MimeMessage mime = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(CommonConfig.ConfigValue("emailname")/*  _emailSettings.Name*/, CommonConfig.ConfigValue("email")/* _emailSettings.EmailId*/);
                mime.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(email.EmailToName, email.EmailToId);
                mime.To.Add(emailTo);
                mime.Subject = email.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = email.EmailBody;
                mime.Body = emailBodyBuilder.ToMessageBody();
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(CommonConfig.ConfigValue("emailhost"), Convert.ToInt16(CommonConfig.ConfigValue("Port")), SecureSocketOptions.Auto);
                smtpClient.Authenticate(CommonConfig.ConfigValue("email"), CommonConfig.ConfigValue("emailpassword"));
                await smtpClient.SendAsync(mime);
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
                var emailreq = _context.EmailRequest.FirstOrDefault(x => x.Email == email.EmailToId);
                emailreq.Status = "Approved";
                _context.Update(emailreq);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
