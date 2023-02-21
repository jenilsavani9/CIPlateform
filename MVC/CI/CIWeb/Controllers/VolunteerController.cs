using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class VolunteerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
