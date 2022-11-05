using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
