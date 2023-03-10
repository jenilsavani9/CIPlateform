using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace CIWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;


        public HomeController(ILogger<HomeController> logger, CiContext db) 
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(long? ID, int? pageIndex, String? searchinput, long[]? fCountries, long[]? fCitys, long[]? fThemes, string? sortOrder)
        {
            int cn = 0;
            int t = 0;
            int c = 0;
            int t1 = 0;

            ViewBag.data = HttpContext.Session.GetString("firstname");

            List<Mission> mission = _db.Missions.ToList();
            List<Mission> newmission = _db.Missions.ToList();
            List<Mission> missionfound = _db.Missions.ToList();


            List<City> Cities = _db.Cities.ToList();
            ViewBag.Cities = Cities;

            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;

            if (sortOrder != null)
            {
                switch (sortOrder)
                {
                    case "newest":
                        ViewBag.TotalMission = _db.Missions.OrderByDescending(o => o.StartDate).Count();
                        return View(_db.Missions.OrderByDescending(o => o.StartDate).ToList());


                    case "oldest":
                        ViewBag.TotalMission = _db.Missions.OrderBy(o => o.EndDate).Count();
                        return View(_db.Missions.OrderBy(o => o.EndDate).ToList());

                    default:
                        return View(_db.Missions.ToList());

                }

            }

            foreach (var item in mission)
            {
                var City = _db.Cities.FirstOrDefault(u => u.CityId == item.CityId);
                var Theme = _db.MissionThemes.FirstOrDefault(u => u.MissionThemeId == item.ThemeId);
            }

            if (!string.IsNullOrEmpty(searchinput))
            {
                mission = _db.Missions.Where(m => m.Title.Contains(searchinput)).ToList();
                ViewBag.Searchinput = Request.Query["searchinput"];
                if (mission.Count() == 0)
                {
                    return RedirectToAction("Nomission", "Home");
                }
            }
            if (fCountries != null && fCountries.Length > 0)
            {
                foreach (var country in fCountries)
                {
                    if (cn == 0)
                    {
                        mission = mission.Where(m => m.CountryId == country + 2500).ToList();
                        cn++;
                    }
                    missionfound = newmission.Where(m => m.CountryId == country).ToList();
                    mission.AddRange(missionfound);

                    ViewBag.SearchCountryId = country;
                    if (ViewBag.SearchCountryId != null)
                    {
                        var A = _db.Countries.FirstOrDefault(m => m.CountryId == country);
                        ViewBag.SearchCountry = A.Name;
                    }
                    if (mission.Count() == 0)
                    {
                        return RedirectToAction("NoMission", "Home", new { @ID = ID });
                    }

                }

            }
            if (fCitys != null && fCitys.Length > 0)
            {
                foreach (var city in fCitys)
                {
                    if (c == 0)
                    {
                        mission = mission.Where(m => m.CityId == city + 2500).ToList();
                        c++;
                    }
                    missionfound = newmission.Where(m => m.CityId == city).ToList();
                    mission.AddRange(missionfound);

                    ViewBag.SearchCityId = city;
                    if (ViewBag.SearchCityId != null)
                    {
                        var A = _db.Cities.FirstOrDefault(m => m.CityId == city);
                        ViewBag.SearchCity = A.Name;
                    }
                    if (mission.Count() == 0)
                    {
                        return RedirectToAction("NoMission", "Home", new { @ID = ID });
                    }

                }

            }
            if (fThemes != null && fThemes.Length > 0)
            {
                foreach (var theme in fThemes)
                {
                    //mission = mission.Where(m => m.ThemeId == theme).ToList();
                    if (t == 0)
                    {
                        mission = mission.Where(m => m.ThemeId == theme + 500).ToList();
                        t++;
                    }

                    missionfound = newmission.Where(m => m.ThemeId == theme).ToList();

                    mission.AddRange(missionfound);
                    if (mission.Count() == 0)
                    {
                        return RedirectToAction("NoMission", "Home");
                    }
                    ViewBag.theme = theme;
                    if (ViewBag.theme != null)
                    {
                        var theme1 = _db.MissionThemes.Where(m => m.MissionThemeId == theme).ToList();
                        if (t1 == 0)
                        {
                            Themes = _db.MissionThemes.Where(m => m.MissionThemeId == theme + 2500).ToList();
                            t1++;
                        }
                        Themes.AddRange(theme1);
                        
                    }
                }
                ViewBag.theme = Themes;
                Themes = _db.MissionThemes.ToList();


            }

            //pagination
            int pageSize = 9;
            int skip = (pageIndex ?? 0) * pageSize;

            var Missions = mission.Skip(skip).Take(pageSize).ToList();

            //if (mission.Count() == 0)
            //{
            //    return RedirectToAction("NoMissionFound", new { });
            //}
            int totalMissions = mission.Count();
            ViewBag.TotalMission = totalMissions;
            ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            ViewBag.CurrentPage = pageIndex ?? 0;



            UriBuilder uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host);
            if (Request.Host.Port.HasValue)
            {
                uriBuilder.Port = Request.Host.Port.Value;
            }
            uriBuilder.Path = Request.Path;

            // Remove the query parameter you want to exclude
            var query = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            query.Remove("pageIndex");
            uriBuilder.Query = query.ToString();



            ViewBag.currentUrl = uriBuilder.ToString();


            return View(Missions);
        }


        // TESTING ENDPOINTS
        public IActionResult Test()
        {
            List<City> Cities = _db.Cities.ToList();
            ViewBag.Cities = Cities;
            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;
            return View();
        }


        [Route("home/api/city")]
        [HttpGet]
        public IActionResult TestCity()
        {
            var mission = _db.Cities.ToList();
            return Ok(mission);
        }

        [Route("home/api/country")]
        [HttpGet]
        public IActionResult TestCountry()
        {
            var mission = _db.Countries.ToList();
            return Ok(mission);
        }

        [Route("home/api/theme")]
        [HttpGet]
        public IActionResult TestTheme()
        {
            var mission = _db.MissionThemes.ToList();
            return Ok(mission);
        }

        [Route("home/api/test")]
        [HttpGet]
        public IActionResult Test2(int? pageIndex)
        {
            var mission = from m in _db.Missions
                          select new
                          {
                              title = m.Title,
                              desc = m.Description,
                              startdate = m.StartDate,
                              enddate = m.EndDate,
                              city = m.City,
                              theme = m.Theme
                          };

            int pageSize = 9;
            int skip = (pageIndex ?? 0) * pageSize;
            var Missions = mission.Skip(skip).Take(pageSize).ToList();
            int totalMissions = mission.Count();
            int TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            int CurrentPage = pageIndex ?? 0;

            return Json(new { Missions, totalMissions, TotalPages, CurrentPage });
        }

        [Route("home/api/search")]
        [HttpGet]
        public IActionResult TestSearch(string? searchTerm, int? pageIndex)
        {

            var mission = _db.Missions.Where(x => x.Title.Contains(searchTerm)).ToList();
            int pageSize = 9;
            int skip = (pageIndex ?? 0) * pageSize;
            var Missions = mission.Skip(skip).Take(pageSize).ToList();
            int totalMissions = mission.Count();
            ViewBag.TotalMission = totalMissions;
            ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            ViewBag.CurrentPage = pageIndex ?? 0;
            return Ok(Missions);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NoMission()
        {
            List<City> Cities = _db.Cities.ToList();
            ViewBag.Cities = Cities;
            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;
            return View();
        }

        public IActionResult ErrorNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}