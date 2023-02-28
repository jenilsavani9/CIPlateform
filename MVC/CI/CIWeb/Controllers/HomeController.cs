using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;


        public HomeController(ILogger<HomeController> logger, CiContext db) 
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ErrorNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}