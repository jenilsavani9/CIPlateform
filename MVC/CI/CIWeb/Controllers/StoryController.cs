using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class StoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;
        private readonly IStoryRepository _repository;

        public StoryController(ILogger<HomeController> logger, CiContext db, IStoryRepository repository)
        {
            _logger = logger;
            _db = db;
            _repository = repository;
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
            return View();

        }

        [HttpGet("/api/story")]
        public IActionResult GetStory(string page)
        {
            var storys = _repository.GetStory(page);

            var pageSize = 3;
            var StoryCount = _repository.StoryCount();
            var totalStoryPage = (int)Math.Ceiling(StoryCount / (double)pageSize);
            var currentStoryPage = int.Parse(page);
            

            return Json(new { storys, totalStoryPage, currentStoryPage });

        }

        [HttpGet("/story/sharestory/getmission")]
        public IActionResult GetAppliedMission()
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            if(userId != null)
            {
                var mission = _repository.GetAppliedMission(userId);

                return Json(new { mission });   
            } 
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("/story/sharestory/saveStory")]
        public IActionResult SaveStory(long mission, string? title, string? date, string? details, string? url, string? status)
        {
            String? userId = HttpContext.Session.GetString("userEmail");


            
            if(userId != null)
            {
                _repository.SaveStory(userId, mission, title, date, details, url, status);
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
            var user = _repository.GetUser(userId);
            ViewBag.user = user;
            return View();
        }
    }
}
