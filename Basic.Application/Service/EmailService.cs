using Basic.Domain.Entity;
using Basic.Domain.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
namespace Basic.Application.Service
{
    public class EmailService : IEmailService
    {
        EmailConfiguration _emailSettings;
        public EmailService(IOptions<EmailConfiguration> options)
        {
            _emailSettings = options.Value;
        }
        public async Task<bool> SendEmail(EmailMessage email)
        {
            try
            {
                MimeMessage mime = new MimeMessage();
                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                mime.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(email.EmailToName, email.EmailToId);
                mime.To.Add(emailTo);
                mime.Subject = email.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = email.EmailBody;
                mime.Body = emailBodyBuilder.ToMessageBody();
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.Auto);
                smtpClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                await smtpClient.SendAsync(mime);
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
