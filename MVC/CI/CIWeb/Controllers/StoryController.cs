using CI.Entities.Models;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class StoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoryRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoryController(ILogger<HomeController> logger, IStoryRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            try
            {
                String? userId = HttpContext.Session.GetString("userEmail");
                if (userId == null)
                {
                    ViewBag.user = null;
                    return RedirectToAction("Login", "User");
                }
                var user = _repository.GetUser(userId);
                ViewBag.user = user;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet("/api/story")]
        public IActionResult GetStory(string page)
        {
            var storys = _repository.GetStory(page);
            var pageSize = 9;
            var StoryCount = _repository.StoryCount();
            var totalStoryPage = (int)Math.Ceiling(StoryCount / (double)pageSize);
            var currentStoryPage = int.Parse(page);
            return Json(new { storys, totalStoryPage, currentStoryPage });
        }

        [HttpGet("/story/sharestory/getmission")]
        public IActionResult GetAppliedMission()
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            if (userId != null)
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
        public IActionResult SaveStory(long mission, string? title, string? date, string? details, string? url, string? status, string? desc, string[]? listOfImage)
        {
            String? userId = HttpContext.Session.GetString("userEmail");



            if (userId != null)
            {
                _repository.SaveStory(userId, mission, title, date, details, url, status, desc, listOfImage);
                return Ok();
            }

            return Ok();
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
            var model = _repository.GetStoryDetails(storyId);

            return Json(new { model });
        }

        [HttpPost("api/static/save")]
        public IActionResult OnPostMyUploader(IFormFile MyUploader)
        {
            if (MyUploader != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                string filePath = Path.Combine(uploadsFolder, MyUploader.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    MyUploader.CopyTo(fileStream);
                }

                return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });
        }

        [HttpGet("/Story/InviteUser")]
        public IActionResult InviteUser(long userId, long storyId)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var res = _repository.InviteUser(userId, storyId, userEmail);

            return Json(new { res });
        }

        [HttpGet("/story/sharestory/draft")]
        public IActionResult DraftStory(long missionId)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var res = _repository.DraftStory(missionId, userEmail);
            List<StoryMedium> media = new List<StoryMedium>();
            if (res != null)
            {
                media = _repository.DraftStoryMedia(res.StoryId);
            }


            return Json(new { res, media });
        }
    }
}
