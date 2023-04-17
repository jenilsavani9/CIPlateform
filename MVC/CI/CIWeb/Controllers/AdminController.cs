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

        public IActionResult Index()
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

        [HttpPost("api/admin/addTheme")]
        public IActionResult AddTheme(ThemeElementModel obj)
        {
            var result = _repository.AddTheme(obj);
            return Json(new { result });
        }

        [HttpGet("api/admin/getTheme")]
        public IActionResult GetThemeWithId(long id)
        {
            var result = _repository.GetThemeWithId(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/editTheme")]
        public IActionResult EditTheme(ThemeElementModel obj)
        {
            var result = _repository.EditTheme(obj);
            return Json(new { result });
        }

        [HttpPost("api/admin/deleteTheme")]
        public IActionResult DeleteTheme(long id)
        {
            var result = _repository.DeleteTheme(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/addSkill")]
        public IActionResult AddSkill(MissionSkillModel obj)
        {
            var result = _repository.AddSkill(obj);
            return Json(new { result });
        }

        [HttpGet("api/admin/getSkill")]
        public IActionResult GetSkillWithId(long id)
        {
            var result = _repository.GetSkillById(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/editSkill")]
        public IActionResult EditSkill(MissionSkillModel obj)
        {
            var result = _repository.EditSkill(obj);
            return Json(new { result });
        }

        [HttpPost("api/admin/deleteSkill")]
        public IActionResult DeleteSkill(long id)
        {
            var result = _repository.DeleteSkill(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/approveMission")]
        public IActionResult ApproveMission(long id)
        {
            var result = _repository.ApproveMission(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/rejectMission")]
        public IActionResult RejectMission(long id)
        {
            var result = _repository.RejectMission(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/approveStory")]
        public IActionResult ApproveStory(long id)
        {
            var result = _repository.ApproveStory(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/rejectStory")]
        public IActionResult RejectStory(long id)
        {
            var result = _repository.RejectStory(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/deleteStory")]
        public IActionResult DeleteStory(long id)
        {
            var result = _repository.DeleteStory(id);
            return Json(new { result });
        }
    }
}
