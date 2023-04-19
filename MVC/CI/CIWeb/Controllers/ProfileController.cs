using CI.Entities.Models;
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
            if(user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult Privacy()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.FindUser(userId);
            ViewBag.user = user;
            if (user != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

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
            _repository.UpdateUserData(user);
            return Ok();
        }

        [HttpGet("/api/profile/country")]
        public ActionResult GetCountry()
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var userCountry = _repository.GetUserCountry(userEmail);
            var country = _repository.GetCountrys();
            return Json(new { country, userCountry });
        }

        [HttpGet("/api/profile/country/{countryId:long}")]
        public ActionResult GetCountryCity(long countryId)
        {
            var country = _repository.GetCountryCity(countryId);
            return Json(new { country });
        }

        [HttpGet("/api/profile/skills")]
        public ActionResult GetUserSkills()
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var skillList = _repository.GetUserSkills(userEmail);
            return Json(new { skillList });
        }

        [HttpGet("/api/profile/changepassword")]
        public ActionResult ChangePassword(string? oldPassword, string? newPassword)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var status = _repository.ChangePassword(userEmail, oldPassword, newPassword);
            return Json(new { status });
        }

        [HttpGet("/api/profile/addskills")]
        public ActionResult SetUserSkills()
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var userSkills = _repository.GetUserSkills(userEmail);
            var NotUserSkills = _repository.GetNotUserSkills(userEmail);
            return Json(new { userSkills, NotUserSkills });
        }

        [HttpGet("/api/profile/saveskills")]
        public JsonResult SaveUserSkills(List<string> skillsToAdd)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            _repository.SaveUserSkills(userEmail, skillsToAdd);
            return Json(new {  });
        }

        [HttpPost("/api/contact")]
        public JsonResult Contact(ContactU obj)
        {
            var status = _repository.ContactUs(obj);
            return Json(new { status });
        }

        // privacy page slug...
        [HttpGet("/api/privacy")]
        public JsonResult PrivacyDetails()
        {
            var result = _repository.PrivacyDetails();
            return Json(new { result });
        }
    }
}
