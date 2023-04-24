using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly CiContext _db;
        private readonly IAdminRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(CiContext db, IAdminRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
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
            var banner = _repository.GetBanner();
            return Json(new {users, missions, missionthemes, missionSkills, missionApplication, missionStory, cmsPages, banner });
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

        [HttpPost("api/admin/addMission")]
        public IActionResult AddMission(AdminMissionModel obj)
        {
            var result = _repository.AddMission(obj);
            return Json(new { result });
        }

        [HttpGet("api/admin/loadMission")]
        public IActionResult LoadMission(long id)
        {
            var result = _repository.LoadMission(id);
            return Json(new { result });
        }

        [HttpPost("api/admin/editMission")]
        public IActionResult EditMission(AdminMissionModel obj)
        {
            var result = _repository.EditMission(obj);
            return Json(new { result });
        }

        [HttpPost("api/admin/deleteMission")]
        public IActionResult DeleteMission(long id)
        {
            var result = _repository.DeleteMission(id);
            return Json(new { result });
        }
        
        [HttpGet("api/admin/getThemes")]
        public IActionResult GetMissionTheme()
        {
            var result = _repository.GetValidMissionThemes();
            return Json(new { result });
        }

        [HttpPost("api/admin/saveImg")]
        public IActionResult SaveImagesToRoot(List<IFormFile> MyUploader)
        {
            if (MyUploader != null)
            {
                for(int i = 0; i < MyUploader.Count; i++)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
                    string filePath = Path.Combine(uploadsFolder, MyUploader[i].FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        MyUploader[i].CopyTo(fileStream);
                    }
                }
                return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });
        }

        [HttpPost("api/admin/saveDoc")]
        public IActionResult SaveDocsToRoot(List<IFormFile> MyUploader)
        {
            if (MyUploader != null)
            {
                for (int i = 0; i < MyUploader.Count; i++)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "document");
                    string filePath = Path.Combine(uploadsFolder, MyUploader[i].FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        MyUploader[i].CopyTo(fileStream);
                    }
                }
                return new ObjectResult(new { status = "success" });
            }
            return new ObjectResult(new { status = "fail" });
        }

        // banner 
        [HttpGet("api/admin/bannerbyid")]
        public IActionResult GetBannerById(long id)
        {
            var result = _repository.GetBannerById(id);
            return Json(new { result });    
        }

        [HttpGet("api/admin/banner")]
        public IActionResult GetBanner()
        {
            var result = _repository.GetBanner();
            return Json(new { result });
        }

        [HttpPost("api/admin/addBanner")]
        public IActionResult AddBanner(Banner obj)
        {
            var result = _repository.AddBanner(obj);
            return Json(new { result });
        }

        [HttpPost("api/admin/deleteBanner")]
        public IActionResult DeleteBanner(long BannerId)
        {
            var result = _repository.DeleteBanner(BannerId);
            return Json(new { result });
        }
    }
}
