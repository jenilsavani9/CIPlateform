using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class StoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
