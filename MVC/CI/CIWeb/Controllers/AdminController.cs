using CI.Entities.Data;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly CiContext _db;
        private readonly IAdminRepository _repository;

        public AdminController(CiContext db, IAdminRepository repository)
        {
            _db = db;
            _repository = repository;
        }

        public IActionResult Index1()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.FindUser(userId);
            if(user!=null && user.Admin == 1)
            {
                ViewBag.user = user;
                return View();
            } else
            {
                return RedirectToAction("ErrorNotFound", "Home");
            }
        }

        [HttpGet("api/admin")]
        public IActionResult GetUsers()
        {
            var users = _repository.GetUsers();
            var missions = _repository.GetMissions();
            var missionthemes = _repository.GetMissionThemes();
            var missionSkills = _repository.GetMissionSkills();
            var missionApplication = _repository.GetMissionApplications();
            var missionStory = _repository.GetMissionStory();
            var cmsPages = _repository.GetCMS();
            return Json(new {users, missions, missionthemes, missionSkills, missionApplication, missionStory, cmsPages });
        }

        [HttpPost("api/admin/addUser")]
        public IActionResult AddUsers(UserProfileModel obj)
        {
            var status = _repository.AddUsers(obj);
            return Json(new { });
        }

        [HttpPost("api/admin/editUser")]
        public IActionResult EditUsers(UserProfileModel obj)
        {
            var status = _repository.EditUsers(obj);
            return Json(new { });
        }

        [HttpGet("api/admin/getUserProfile")]
        public IActionResult GetUserProfile(long id)
        {
            var result = _repository.GetUserProfile(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/deleteUserProfile")]
        public IActionResult DeleteUserProfile(long id)
        {
            var result = _repository.DeleteUserProfile(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/addCMS")]
        public IActionResult AddCms(CMSModel obj)
        {
            var result = _repository.AddCms(obj);
            return Json(new { result });
        }

        [HttpGet("api/admin/getCMS")]
        public IActionResult GetCMSWithId(long id)
        {
            var result = _repository.GetCMSWithId(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/editCMS")]
        public IActionResult EditCMS(CMSModel obj)
        {
            var result = _repository.EditCMS(obj);
            return Json(new { result });
        }
        
        [HttpPost("api/admin/deleteCMS")]
        public IActionResult DeleteCMS(long id)
        {
            var result = _repository.DeleteCMS(id);
            return Json(new { result });
        }
    }
}
