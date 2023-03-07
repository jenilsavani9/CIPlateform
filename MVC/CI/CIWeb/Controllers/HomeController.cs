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

        public IActionResult Index(int? pageNumber, string? searchTerm, string? sortOrder)
        {
            int pageSize = 9;
            ViewBag.data = HttpContext.Session.GetString("firstname");

            List<Mission> mission = _db.Missions.ToList();

            if (searchTerm == null && sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "title":
                        return View(PaginationList<Mission>.Create(_db.Missions.OrderBy(o => o.Title).ToList(), pageNumber ?? 1, pageSize));
                        
                    case "deadline":
                        return View(PaginationList<Mission>.Create(_db.Missions.OrderBy(o => o.EndDate).ToList(), pageNumber ?? 1, pageSize));
                        
                    default:
                        return View(PaginationList<Mission>.Create(_db.Missions.ToList(), pageNumber ?? 1, pageSize));
                        
                }
                
            }

            if (searchTerm == null)
            {
                return View(PaginationList<Mission>.Create(_db.Missions.ToList(), pageNumber ?? 1, pageSize));
            }

            else
            {
                var isFound = _db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()));
                var isShortDescFound = _db.Missions.Where(m => m.ShortDescription.ToLower().Contains(searchTerm.ToLower()));
                if (isFound.Any() || isShortDescFound.Any())
                {
                    return View(PaginationList<Mission>.Create(_db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()) || m.ShortDescription.ToLower().Contains(searchTerm.ToLower())
                    ).ToList(), pageNumber ?? 1, pageSize));
                }

                else
                {
                    return RedirectToAction("NoMission", "Home");
                }
            }
        }

        public IActionResult Test(int? pageNumber, string? searchTerm)
        {
            int pageSize = 2;

            if (searchTerm == null)
            {
                return View(PaginationList<Mission>.Create(_db.Missions.ToList(), pageNumber ?? 1, pageSize));
            }

            else
            {
                var isFound = _db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()));
                var isShortDescFound = _db.Missions.Where(m => m.ShortDescription.ToLower().Contains(searchTerm.ToLower()));
                if (isFound.Any() || isShortDescFound.Any())
                {

                    return View(PaginationList<Mission>.Create(_db.Missions.Where(m => m.Title.ToLower().Contains(searchTerm.ToLower()) || m.ShortDescription.ToLower().Contains(searchTerm.ToLower())
                    ).ToList(), pageNumber ?? 1, pageSize));
                }

                else
                {
                    return RedirectToAction("Noission", "Home");
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NoMission()
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