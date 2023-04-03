using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfileRepository _repository;

        public ProfileController(ILogger<HomeController> logger, IProfileRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public ActionResult Index()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.FindUser(userId);
            ViewBag.user = user;
            return View();
        }

        [HttpGet("/getUserProfile")]
        public ActionResult GetUserProfile()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUserProfile(userId);
            return Json(new { user });
        }

        [HttpPost("/getUserProfile")]
        public ActionResult SetUserProfile(UserProfileModel user)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            _repository.UpdateUserData(user);
            return Ok();
        }
    }
}
