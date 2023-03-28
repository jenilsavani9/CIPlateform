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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoryController(ILogger<HomeController> logger, CiContext db, IStoryRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _db = db;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult SaveStory(long mission, string? title, string? date, string? details, string? url, string? status, string? desc)
        {
            String? userId = HttpContext.Session.GetString("userEmail");


            
            if(userId != null)
            {
                _repository.SaveStory(userId, mission, title, date, details, url, status, desc);
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

        [HttpGet("/story/{id:int}")]
        public IActionResult StoryDetails(int storyId)
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

        [HttpGet("api/story/{storyId:long}")]
        public IActionResult GetStoryDetails(long storyId)
        {
            StoryDetailsModel model = new StoryDetailsModel();
            var story = _db.Stories.Where(s => s.StoryId == storyId).FirstOrDefault();
            var mission = _db.Missions.Where(m => m.MissionId == story.MissionId).FirstOrDefault();
            var user = _db.Users.Where(u => u.UserId == story.UserId).FirstOrDefault();

            model.whyIVolunteer = user.WhyIVolunteer;
            model.missionTitle = mission.Title;
            model.avatar = user.Avatar;
            model.storyDetails = story.Description;
            model.userName = user.FirstName + " " + user.LastName;
            model.missionId = mission.MissionId;

            return Json(new { model });
        }

        [HttpPost("api/static/save")]
        public IActionResult OnPostMyUploader(IFormFile MyUploader, IFormCollection data)
        {
            if (MyUploader != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                string filePath = Path.Combine(uploadsFolder, MyUploader.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    MyUploader.CopyTo(fileStream);
                }
                StoryMedium storyMedia = new StoryMedium();
                var fileName = MyUploader.FileName;
                var mission = data["mission"][0];
                var storyType = MyUploader.ContentType;
                storyMedia.StoryType = storyType;
                storyMedia.StoryPath = fileName;
                storyMedia.StoryId = (long)2;

                return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });
        }
    }
}
