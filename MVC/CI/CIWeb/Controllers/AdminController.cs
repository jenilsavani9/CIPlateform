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
            return Json(new {users, missions, missionthemes, missionSkills, missionApplication, missionStory });
        }

        [HttpPost("api/admin/addUser")]
        public IActionResult AddUsers(UserProfileModel obj)
        {
            var status = _repository.AddUsers(obj);
            return Json(new { });
        }
    }
}
