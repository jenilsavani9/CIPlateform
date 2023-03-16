using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace CIWeb.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly CiContext _db;

        public VolunteerController(CiContext db)
        {
            _db = db;
        }


        //[HttpGet("/missions/{id:int}")]
        //public IActionResult Index(int? id)
        //{
        //    String? userId = HttpContext.Session.GetString("userEmail");

        //    var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
        //    ViewBag.user = user;

        //    int? missionId = id;
        //    Mission = _db.Missions.Where(m => m.MissionId == missionId).ToList();
        //    var city = _db.Cities.Where(c => c.CityId == Mission[0].CityId).ToList();
        //    var country = _db.Countries.Where(c => c.CountryId == city[0].CountryId).ToList();
        //    var theme = _db.MissionThemes.Where(c => c.MissionThemeId == Mission[0].ThemeId).ToList();
        //    ViewBag.mission = Mission;
        //    ViewBag.city = city;
        //    ViewBag.country = country;
        //    ViewBag.theme = theme;

        //    // related missions
        //    var relatedMission = _db.Missions.Where(m => m.CityId == Mission[0].CityId).Take(3).ToList();
        //    if(relatedMission.Count < 3)
        //    {
        //        relatedMission = _db.Missions.Where(m => m.CountryId == Mission[0].CountryId).Take(3).ToList();
        //    }
        //    if (relatedMission.Count < 3)
        //    {
        //        relatedMission = _db.Missions.Where(m => m.ThemeId == Mission[0].ThemeId).Take(3).ToList();
        //    }
        //    ViewBag.relatedMission = relatedMission;
        //    return View();
        //}

        [HttpGet("/missions/{id:int}")]
        public IActionResult Index1(int? id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
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
            var mission = _db.Missions.Where(m => m.MissionId == missionId).ToList();
            var city = _db.Cities.Where(c => c.CityId == mission[0].CityId).ToList();
            var country = _db.Countries.Where(c => c.CountryId == city[0].CountryId).ToList();
            var theme = _db.MissionThemes.Where(c => c.MissionThemeId == mission[0].ThemeId).ToList();
            VolunteerMissionModel.Mission = mission;
            VolunteerMissionModel.City = city;
            VolunteerMissionModel.Country = country;
            VolunteerMissionModel.Themes = theme;

            // related missions
            var relatedMission = _db.Missions.Where(m => m.CityId == mission[0].CityId).Take(3).ToList();
            if (relatedMission.Count < 3)
            {
                relatedMission = _db.Missions.Where(m => m.CountryId == mission[0].CountryId).Take(3).ToList();
            }
            if (relatedMission.Count < 3)
            {
                relatedMission = _db.Missions.Where(m => m.ThemeId == mission[0].ThemeId).Take(3).ToList();
            }
            VolunteerMissionModel.relatedMission = relatedMission;
            return Json(new { VolunteerMissionModel });
        }

        

        [HttpGet("/missions/{id:int}/addFavorite")]
        public IActionResult AddToFavorite(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            
            int missionId = id;

            var tuser = _db.FavoriteMissions.Where(m => m.UserId == user.UserId && m.MissionId == missionId).ToList();
            if (tuser.Any())
            {
                var temp = _db.FavoriteMissions.Where(m => m.UserId == user.UserId && m.MissionId == missionId).First();
                _db.FavoriteMissions.Remove(temp);
                _db.SaveChanges();
                return Json("Remove");
            }
            else
            {
                _db.FavoriteMissions.Add(new FavoriteMission { UserId = user.UserId, MissionId = missionId });
                _db.SaveChanges();
                return Json("Add");
            }

        }

        [HttpGet("/missions/{id:int}/checkFavorite")]
        public IActionResult CheckAddToFavorite(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
           
            int missionId = id;

            var tuser = _db.FavoriteMissions.Where(m => m.UserId == user.UserId && m.MissionId == missionId).ToList();
       
            if (tuser.Any())
            {
                return Json("IN");
            } else
            {
                return Json("Out");
            }
        }

        [HttpGet("/missions/{id:int}/rating/{rate:int}")]
        public IActionResult Rating(int id, int rate)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            int missionId = id;

            var tuser = _db.MissionRatings.Where(m => m.UserId == user.UserId && m.MissionId == missionId).ToList();
            if(tuser.Any())
            {
                var temp = _db.MissionRatings.Where(m => m.UserId == user.UserId && m.MissionId == missionId).First();
                _db.MissionRatings.Remove(temp);
                _db.MissionRatings.Add(new MissionRating { UserId = user.UserId, MissionId = missionId, Rating = rate.ToString() });
                _db.SaveChanges();
                return Json(rate.ToString());
            } else
            {
                _db.MissionRatings.Add(new MissionRating { UserId = user.UserId, MissionId = missionId, Rating = rate.ToString() });
                _db.SaveChanges();
                return Ok(rate.ToString());
            }

        }

        [HttpGet("/missions/{id:int}/checkrating")]
        public IActionResult CheckRating(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            int missionId = id;

            var tmission = _db.MissionRatings.Where(m => m.UserId == user.UserId && m.MissionId == missionId).ToList();

            if (tmission.Any())
            {
                return Json(tmission);
            }
            else
            {
                return Json("Not Found");
            }
        }

        [HttpGet("/missions/{id:int}/ratingGroup")]
        public IActionResult RatingGroup(int id)
        {

            var avg = 0;
            int missionId = id;

            var tmission = _db.MissionRatings.Where(m =>  m.MissionId == missionId).ToList();

            for(var i=0; i<tmission.Count; i++)
            {
                avg += int.Parse(tmission[i].Rating);
            }

            if(tmission.Count() > 0)
            {
                avg /= tmission.Count();
            } else
            {
                avg = 0;
            }

            if (tmission.Any())
            {
                return Json(new { avg = avg.ToString(), tUser = tmission.Count()});
            }
            else
            {
                return Json(new { avg = avg.ToString(), tUser = tmission.Count() });
            }
        }

        [HttpPost("/recommand/{id:int}/{missionId:int}")]
        public IActionResult Recommand(int id, int missionId)
        {
            var user = _db.Users.Where(u => u.UserId == id).FirstOrDefault();
            var checkRecommend = _db.MissionInvites.Where(u => u.FromUserId == user.UserId && u.MissionId == missionId && u.ToUserId == id).FirstOrDefault();
            if(checkRecommend == null)
            {
                _db.MissionInvites.Add(new MissionInvite { ToUserId = user.UserId, MissionId = missionId, FromUserId = user.UserId });
                _db.SaveChanges();
                // Send an email with the password reset link to the user's email address
                var resetLink = "https://localhost:44398/missions/" + missionId.ToString();
                // Send email to user with reset password link
                // ...
                var fromAddress = new MailAddress("jenilsavani8@gmail.com", "CI Platform");
                var toAddress = new MailAddress(user.Email);
                var subject = "Recommendation for Joining In Mission";
                var body = $"Hi,<br /><br />Please click on the following link to Joining In Mission:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("jenilsavani8@gmail.com", "bwgnmdxyggqrylsu"),
                    EnableSsl = true
                };
                smtpClient.Send(message);
                return Json(new { message = "sent"});
            } else
            {
                return Json(new { message = "sent" });
            }

        }

        [HttpGet("/mission/{missionId:int}/organization")]
        public IActionResult Organization(int missionId)
        {
            var mission = _db.Missions.Where(m => m.MissionId == missionId).FirstOrDefault();
            if (mission == null)
            {
                return Json(new { message = "No Mission Found" });
            }
            else
            {
                return Json(new { mission });
            }
            

        }

    }
}
