using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
