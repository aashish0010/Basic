using Basic.Application.Function;
using Basic.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Basic.Controllers
{
    public class ServerController : Controller
    {
        private readonly IHubContext<NotificationHub> _notify;

        public ServerController(IHubContext<NotificationHub> notify)
        {
            _notify = notify;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.number = Data.DataList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ChatBox chat)
        {
            await _notify.Clients.All.SendAsync("ReceiveMsg", chat.Message);
            return View();
        }

    }
}
