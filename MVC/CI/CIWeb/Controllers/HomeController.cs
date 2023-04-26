using CI.Entities.Models;
using CI.Repository.Interface;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _repository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.FindUser(userId);
            if(user != null)
            {
                ViewBag.user = user;

                var Countries = _repository.GetCountry();
                var Themes = _repository.GetTheme();
                var Skills = _repository.GetSkill();

                ViewBag.countries = Countries;
                ViewBag.themes = Themes;
                ViewBag.skills = Skills;
            } 
            else
            {
                ViewData.Clear();
                var Countries = _repository.GetCountry();
                var Themes = _repository.GetTheme();
                var Skills = _repository.GetSkill();

                ViewBag.countries = Countries;
                ViewBag.themes = Themes;
                ViewBag.skills = Skills;
            }
            
            return View();
        }

        [HttpGet("/api/missions")]
        public IActionResult GetMission(string? searchQuery, long[] FCountries, long[] FCities, long[] FThemes, long[] FSkills, int? pageIndex, string sortOrder)
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            var model = _repository.GetFilterMissions(userId, searchQuery, FCountries, FCities, FThemes, FSkills, pageIndex, sortOrder);

            return Ok(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NoMission()
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            var user = _repository.FindUser(userId);
            ViewBag.user = user;

            List<City> Cities = _repository.GetCities();
            ViewBag.cities = Cities;
            List<Country> Country = _repository.GetCountry();
            ViewBag.countries = Country;
            List<MissionTheme> Themes = _repository.GetTheme();
            ViewBag.Themes = Themes;
            ViewBag.cityElements = Cities;
            ViewBag.themeElements = Themes;
            ViewBag.countryElements = Country;
            return View();
        }

        [Route("/Error")]
        public IActionResult ErrorNotFound()
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            var user = _repository.FindUser(userId);
            ViewBag.user = user;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}