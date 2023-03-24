using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
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

            
            if (userId == null) 
            {
                return RedirectToAction("Login", "User");
            }
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
            var totalStoryPage = (int)Math.Ceiling(story.Count() / (double)pageSize); 

            var currentStoryPage = int.Parse(page);
            story = story.Skip(int.Parse(page) * pageSize).Take(pageSize);

            return Json(new { story, totalStoryPage, currentStoryPage });

        }

        [HttpGet("/story/sharestory/getmission")]
        public IActionResult GetAppliedMission()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();

            if(user != null)
            {
                var missionApplication = _db.MissionApplications.Where(a => a.UserId == user.UserId).ToList();
                var mission = new List<ShareMissionApplyMissionModel>();
                foreach(var item in missionApplication)
                {
                    var tempMission = _db.Missions.Where(m => m.MissionId == item.MissionId).SingleOrDefault();
                    mission.Add(new ShareMissionApplyMissionModel
                    {
                        id = item.MissionId,
                        title = tempMission?.Title
                    });
                }

                return Json(new { mission });
            } 
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("/story/sharestory/saveStory")]
        public IActionResult SaveStory( long mission, string? title, string? date, string? details, string? url, string? status)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            if(user != null)
            {
                

                if(status == "save")
                {
                    var story = new Story();
                    story.MissionId = mission;
                    story.Title = title;
                    story.UserId = user.UserId;
                    story.PublishedAt = DateTime.Now;
                    _db.Stories.Add(story);
                } 
                else
                {
                    var story = _db.Stories.Where(s => s.MissionId == mission && s.Title == title && s.Status == "draft").FirstOrDefault();
                    if(story != null)
                    {
                        story.Status = "pending";
                    }
                    
                }

                _db.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }

        public IActionResult ShareStory()
        {
            String? userId = HttpContext.Session.GetString("userEmail");


            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            ViewBag.user = user;
            return View();
        }
    }
}
