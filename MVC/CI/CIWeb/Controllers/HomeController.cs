using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

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

            List<City> Cities = _db.Cities.ToList();
            ViewBag.Cities = Cities;

            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;

            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;

            List<Mission> mission = _db.Missions.ToList();
            ViewBag.count = _db.Missions.Count();

            foreach (var item in mission)
            {
                var City = _db.Cities.FirstOrDefault(C => C.CityId == item.CityId);
                var Theme = _db.MissionThemes.FirstOrDefault(t => t.MissionThemeId == item.ThemeId);
            }

            if (searchTerm == null && sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "newest":
                        return View(PaginationList<Mission>.Create(_db.Missions.OrderByDescending(o => o.CreatedAt).ToList(), pageNumber ?? 1, pageSize));

                    case "oldest":
                        return View(PaginationList<Mission>.Create(_db.Missions.OrderBy(o => o.CreatedAt).ToList(), pageNumber ?? 1, pageSize));
                        
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
            List<City> Cities = _db.Cities.ToList();
            ViewBag.Cities = Cities;

            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;

            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;
            return View();
        }

        [Route("home/api/city")]
        [HttpGet]
        public IActionResult TestCity()
        {
            var mission = _db.Cities.ToList();
            return Ok(mission);
        }

        [Route("home/api/test")]
        [HttpGet]
        public IActionResult Test2()
        {

            var mission = from m in _db.Missions
                          select new 
                          {
                            title = m.Title,
                            desc = m.Description,
                            startdate = m.StartDate,
                            enddate = m.EndDate,
                            city = m.City,
                            theme = m.Theme
                          };


            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //    WriteIndented = true
            //};

            return Ok(mission);
        }

        [Route("home/api/search")]
        [HttpGet]
        public IActionResult TestSearch(string? searchTerm)
        {

            var mission = _db.Missions.Where(x => x.Title.Contains(searchTerm)).ToList();


            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //    WriteIndented = true
            //};

            return Ok(mission);
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