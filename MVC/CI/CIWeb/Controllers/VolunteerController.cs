﻿using CI.Entities.Data;
using CI.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CIWeb.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly CiContext _db;

        private List<Mission> Mission = new List<Mission>();
        private List<City> Citie = new List<City>();
        private List<MissionTheme> Themes = new List<MissionTheme>();
        private List<Country> Country = new List<Country>();


        public VolunteerController(CiContext db)
        {
            _db = db;
        }


        [HttpGet("/missions/{id:int}")]
        public IActionResult Index(int? id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            ViewBag.user = user;

            int? missionId = id;
            Mission = _db.Missions.Where(m => m.MissionId == missionId).ToList();
            var city = _db.Cities.Where(c => c.CityId == Mission[0].CityId).ToList();
            var country = _db.Countries.Where(c => c.CountryId == city[0].CountryId).ToList();
            var theme = _db.MissionThemes.Where(c => c.MissionThemeId == Mission[0].ThemeId).ToList();
            ViewBag.mission = Mission;
            ViewBag.city = city;
            ViewBag.country = country;
            ViewBag.theme = theme;

            // related missions
            var relatedMission = _db.Missions.Where(m => m.CityId == Mission[0].CityId).Take(3).ToList();
            if(relatedMission.Count < 3)
            {
                relatedMission = _db.Missions.Where(m => m.CountryId == Mission[0].CountryId).Take(3).ToList();
            }
            if (relatedMission.Count < 3)
            {
                relatedMission = _db.Missions.Where(m => m.ThemeId == Mission[0].ThemeId).Take(3).ToList();
            }
            ViewBag.relatedMission = relatedMission;
            return View();
        }

        [HttpGet("/missions/{id:int}/addFavorite")]
        public IActionResult AddToFavorite(int id)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            var status = _db.Users.ToList();
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
            var status = _db.Users.ToList();
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
    }
}
