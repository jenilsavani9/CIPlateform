using CI.Entities.Data;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly CiContext _db;

        private readonly IVolunteerRepository _repository;

        public VolunteerController(CiContext db, IVolunteerRepository repository)
        {
            _db = db;
            _repository = repository;
        }

        [HttpGet("/missions/{id:int}")]
        public IActionResult Index(int? id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);
            ViewBag.user = user;
            return View();
        }

        [HttpGet("/api/missions/{id:int}")]
        public IActionResult TestIndex(int? id)
        {

            VolunteerMissionModel VolunteerMissionModel = new VolunteerMissionModel();
            String? userId = HttpContext.Session.GetString("userEmail");

            VolunteerMissionModel.user = _db.Users.Where(e => e.Email == userId);

            int? missionId = id;
            //var mission = _db.Missions.Where(m => m.MissionId == missionId).ToList();
            var mission = _repository.GetMissions(missionId);
            if(mission != null)
            {
                var city = _repository.GetCitys(mission[0].CityId);
                if(city != null)
                {
                    var country = _repository.GetCountrys(city[0].CountryId);
                    var theme = _repository.GetThemes(mission[0].ThemeId);
                    VolunteerMissionModel.Mission = mission;
                    VolunteerMissionModel.City = city;
                    VolunteerMissionModel.Country = country;
                    VolunteerMissionModel.Themes = theme;

                    //related missions
                    var relatedMission = _repository.RelatedMissions(mission[0].CityId, city[0].CountryId, mission[0].ThemeId);
                    VolunteerMissionModel.relatedMission = relatedMission;
                }
                
            }
            
            return Json(new { VolunteerMissionModel });
        }

        [HttpGet("/missions/{id:int}/addFavorite")]
        public IActionResult AddToFavorite(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);

            int missionId = id;

            if (user != null)
            {

                var tuser = _repository.AddToFavorite(user.UserId, missionId);
                if (tuser == false)
                {

                    return Json("Remove");
                }
                else
                {

                    return Json("Add");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("/missions/{id:int}/checkFavorite")]
        public IActionResult CheckAddToFavorite(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);

            int missionId = id;

            if (user != null)
            {
                var tuser = _repository.CheckAddToFavorite(user.UserId, missionId);

                if (tuser != null && tuser.Any())
                {
                    return Json("IN");
                }
                else
                {
                    return Json("Out");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("/missions/{id:int}/rating/{rate:int}")]
        public IActionResult Rating(int id, int rate)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);
            int missionId = id;

            if (user != null)
            {
                var tuser = _repository.Rating(user.UserId, rate, missionId);
                return Json(tuser);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("/missions/{id:int}/checkrating")]
        public IActionResult CheckRating(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);
            int missionId = id;

            if (user != null)
            {
                var tmission = _repository.CheckRating(user.UserId, missionId);

                if (tmission.Any())
                {
                    return Json(tmission);
                }
                else
                {
                    return Json("Not Found");
                }
            }
            return BadRequest();
        }

        [HttpGet("/missions/{id:int}/ratingGroup")]
        public IActionResult RatingGroup(int id)
        {

            var avg = 0;
            int missionId = id;

            var tmission = _repository.RatingGroup(missionId);

            for (var i = 0; i < tmission.Count; i++)
            {
                avg += int.Parse(tmission[i].Rating);
            }

            if (tmission.Count() > 0)
            {
                avg /= tmission.Count();
            }
            else
            {
                avg = 0;
            }

            if (tmission.Any())
            {
                return Json(new { avg = avg.ToString(), tUser = tmission.Count() });
            }
            else
            {
                return Json(new { avg = avg.ToString(), tUser = tmission.Count() });
            }
        }

        [HttpPost("/recommand/{id:int}/{missionId:int}")]
        public IActionResult Recommand(int id, int missionId)
        {
            var mail = _repository.Recommand(id, missionId);

            if (mail == true)
            {
                return Json(new { message = "sent" });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("/mission/{missionId:int}/organization")]
        public IActionResult Organization(int missionId)
        {
            var mission = _repository.Organization(missionId);
            return Json(new { mission });
        }

        [HttpGet("/mission/{missionId:int}/comments")]
        public IActionResult GetComments(int missionId)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);

            if (user != null)
            {
                var CommentsModel = _repository.GetComments(missionId);
                return Json(new { CommentsModel });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("/mission/{missionId:int}/comments")]
        public IActionResult addComments(int missionId, string? getTextarea)
        {

            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);

            if (user != null)
            {
                var comment = _repository.AddComments(user.UserId, missionId, getTextarea);
                if (comment == true)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpGet("/missions/{missionId:int}/getapply")]
        public IActionResult GetApplyMission(int missionId)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);

            if (user != null)
            {
                var mission = _repository.GetApplyMission(user.UserId, missionId);
                if (mission != null)
                {
                    if (mission.ApprovalStatus == "pending")
                    {
                        return Json(new { message = "Pending" });
                    }
                    else
                    {
                        return Json(new { message = "Apply" });
                    }
                }
                else
                {
                    return Json(new { message = "NotApply" });
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("/missions/{missionId:int}/apply")]
        public IActionResult ApplyMission(int missionId)
        {

            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userId);

            if (user != null)
            {
                var mission = _repository.ApplyMission(user.UserId, missionId);
                if (mission == true)
                {
                    return Json(new { message = "Apply" });
                }
                return Json(new { message = "NotApply" });
            }
            else
            {
                return Json(new { message = "NotApply" });
            }
        }

        [HttpGet("/missions/{missionId:int}/getVolunteers")]
        public IActionResult GetVolunteers(int missionId, string page)
        {

            if (int.Parse(page) < 0)
            {
                page = "0";
            }
            // pagination for recent volunteers
            ViewBag.RecentVolunttersPage = page;
            var RecentVolunteerModel = _repository.GetVolunteers(missionId, page);

            return Json(new { RecentVolunteerModel });

        }

        [HttpGet("/missions/{missionId:int}/getMissionSkill")]
        public IActionResult GetSkillsAndDays(int missionId)
        {
            var missionSkills = _repository.GetMissionSkills(missionId);
            var missionDays = _repository.GetMissionDays(missionId);

            return Json(new { missionSkills, missionDays });
        }

        [HttpGet("/missions/{missionId:int}/getMissionDocument")]
        public IActionResult GetMissionDocument(int missionId)
        {
            var document = _repository.GetMissionDocument(missionId);
            return Json(new { document });
        }
        
        [HttpGet("/missions/{missionId:int}/getMissionMedia")]
        public IActionResult GetMissionMedia(int missionId)
        {
            var result = _repository.GetMissionMedia(missionId);
            return Json(new { result });
        }
    }
}
