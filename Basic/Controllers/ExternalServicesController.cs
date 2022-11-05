using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    public class ExternalServicesController : Controller
    {
        private readonly IEmailService _emailService;
        public ExternalServicesController(IEmailService email)
        {
            _emailService = email;
        }
        public IActionResult Index(EmailMessage message)
        {
            //message.EmailToId = "wostibinay83@gmail.com";
            //message.EmailToName = "manniswosti1566@gmail.com";
            //message.EmailBody = "<h4>Dear Sir/Madam</h4><p style=\"margin-left:80px\">Here is your pinno code:</p>if you have any issue contact<a href=\"\" style=\"color:blue\"> here</a><br><br><b style=\"color:blue\">Your Sincere,<br>aashish</b>";
            //message.EmailSubject = "Enjoy";
            _emailService.SendEmail(message);
            return View();
        }

    }
}
