using CI.Entities.Data;
using CI.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class StoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;

        public StoryController(ILogger<HomeController> logger, CiContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            ViewBag.user = user;

            List<City> Cities = _db.Cities.ToList();
            ViewBag.cities = Cities;
            List<Country> Country = _db.Countries.ToList();
            ViewBag.countries = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;
            List<Skill> Skills = _db.Skills.ToList();
            ViewBag.skills = Skills;
            ViewBag.skillElements = Skills;
            ViewBag.cityElements = Cities;
            ViewBag.themeElements = Themes;
            ViewBag.countryElements = Country;

            return View();

        }

        [HttpGet("/api/story")]
        public IActionResult GetStory(string page)
        {

            var story = from s in _db.Stories
                        select new
                        {
                            story = s,
                            user = s.User,
                            mission = s.Mission,
                            theme = s.Mission.Theme
                        };

            int pageSize = 3;
            var totalStoryPage = story.Count() / pageSize;
            var currentStoryPage = int.Parse(page);
            story = story.Skip(int.Parse(page) * pageSize).Take(pageSize);

            return Json(new { story, totalStoryPage, currentStoryPage });

        }

        public IActionResult ShareStory(string page)
        {
            return View();
        }
    }
}
