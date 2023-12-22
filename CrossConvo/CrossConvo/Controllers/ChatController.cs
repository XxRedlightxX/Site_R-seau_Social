using Microsoft.AspNetCore.Mvc;

namespace CrossConvo.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
