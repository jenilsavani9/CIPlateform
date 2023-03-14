using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace CIWeb.Controllers
{
    public class HomeController : Controller
    {
        private List<MissionViewModel> missionsVMList = new List<MissionViewModel>();

        private List<Mission> Missions = new List<Mission>();

        private List<Mission> FinalMissionsList = new List<Mission>();

        private List<Country> Countries = new List<Country>();

        private List<City> Cities = new List<City>();

        private List<MissionTheme> Themes = new List<MissionTheme>();

        private List<Country> countryElements = new List<Country>();

        private List<MissionSkill> missionSkills = new List<MissionSkill>();

        private List<Skill> Skills = new List<Skill>();

        private List<City> cityElements = new List<City>();

        private List<MissionTheme> themeElements = new List<MissionTheme>();

        private List<Skill> skillElements = new List<Skill>();

        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;


        public HomeController(ILogger<HomeController> logger, CiContext db) 
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(long id, int? pageIndex, string searchQuery, long[] FCountries, long[] FCities, long[] FThemes, long[] FSkills, string sortOrder)
        {

            // Check if user is logged in
            String? userId = HttpContext.Session.GetString("userEmail");
            
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            ViewBag.user = user;
            ViewBag.Request = Request;
            Missions = _db.Missions.ToList();
            Countries = _db.Countries.ToList();
            Themes = _db.MissionThemes.ToList();
            Skills = _db.Skills.ToList();
            ViewBag.countries = Countries;
            ViewBag.themes = Themes;
            ViewBag.skills = Skills;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                Missions = _db.Missions.ToList();
                Missions = Missions.Where(m => m.Title.ToLower().Contains(searchQuery.ToLower())).ToList();
                FinalMissionsList.AddRange(Missions);
                ViewBag.searchQuery = Request.Query["searchQuery"];
            }

            if (FCountries != null && FCountries.Length > 0)
            {
                foreach (var country in FCountries)
                {
                    Missions = _db.Missions.ToList();

                    Missions = Missions.Where(m => m.CountryId == country).ToList();
                    FinalMissionsList.AddRange(Missions);
                    var countryElement = _db.Countries.Where(m => m.CountryId == country).ToList();
                    countryElements.AddRange(countryElement);
                    var cities = _db.Cities.Where(m => m.CountryId == country).ToList();
                    Cities.AddRange(cities);
                }
                ViewBag.countryElements = countryElements;
                ViewBag.cities = Cities;
                ViewBag.FCountries = Request.Query["FCountries"];
            }

            if (FCities != null && FCities.Length > 0)
            {

                var tempFinalList = new List<Mission>();
                foreach (var city in FCities)
                {
                    if (FinalMissionsList.Count > 0)
                    {
                        Missions = FinalMissionsList;
                    }
                    else
                    {
                        Missions = _db.Missions.ToList();
                    }

                    Missions = Missions.Where(m => m.CityId == city).ToList();
                    var cityElement = _db.Cities.Where(m => m.CityId == city).ToList();
                    cityElements.AddRange(cityElement);
                    tempFinalList.AddRange(Missions);

                }
                FinalMissionsList = tempFinalList;
                ViewBag.cityElements = cityElements;
                ViewBag.FCities = Request.Query["FCities"];
            }

            if (FThemes != null && FThemes.Length > 0)
            {
                var tempFinalList = new List<Mission>();
                foreach (var theme in FThemes)
                {
                    if (FinalMissionsList.Count > 0)
                    {
                        Missions = FinalMissionsList;
                    }
                    else
                    {
                        Missions = _db.Missions.ToList();
                    }
                    Missions = Missions.Where(m => m.ThemeId == theme).ToList();
                    var themeElement = _db.MissionThemes.Where(m => m.MissionThemeId == theme).ToList();
                    themeElements.AddRange(themeElement);
                    tempFinalList.AddRange(Missions);
                }
                FinalMissionsList = tempFinalList;
                ViewBag.FThemes = Request.Query["FThemes"];
                ViewBag.themeElements = themeElements;
            }

            if (FSkills != null && FSkills.Length > 0)
            {
                var element = new List<MissionSkill>();
                var tempFinalList = new List<Mission>();
                foreach (var skill in FSkills)
                {
                    var tempList = _db.MissionSkills.Where(m => m.SkillId == skill).ToList();
                    element.AddRange(tempList);
                    if (FinalMissionsList.Count > 0)
                    {
                        Missions = FinalMissionsList;
                    }
                    else
                    {
                        Missions = _db.Missions.ToList();
                    }
                    Missions = Missions.Where(m => m.ThemeId == skill).ToList();
                    var skillElement = _db.Skills.Where(m => m.SkillId == skill).ToList();
                    skillElements.AddRange(skillElement);
                    tempFinalList.AddRange(Missions);
                }
                FinalMissionsList = tempFinalList;
                ViewBag.FSkills = Request.Query["FSkill"];
                ViewBag.skillElements = skillElements;
            }



            //Pagination
            int pageSize = 9; // change this to your desired page size
            int skip = (pageIndex ?? 0) * pageSize;
            if (FinalMissionsList.Count() != 0)
            {
                var missions = FinalMissionsList.ToList();
                foreach (var mission in missions)
                {
                    City city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                    MissionTheme theme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                    string[] startDateNtime = mission.StartDate.ToString().Split(' ');
                    string[] endDateNtime = mission.EndDate.ToString().Split(' ');
                    var ratings = _db.MissionRatings.Where(e => e.MissionId == mission.MissionId).ToList();
                    var rating = 0;
                    var sum = 0;
                    foreach (var entry in ratings)
                    {
                        sum = sum + int.Parse(entry.Rating);

                    }
                    //rating = sum / ratings.Count;
                    missionsVMList.Add(new MissionViewModel
                    {
                        MissionId = mission.MissionId,
                        Title = mission.Title,
                        Description = mission.Description,
                        City = city.Name,
                        Organization = mission.OrganizationName,
                        Theme = theme.Title,
                        //Rating = rating,
                        StartDate = (DateTime)mission.StartDate,
                        EndDate = (DateTime)mission.EndDate,
                        missionType = mission.MissionType,
                        isFavrouite = (user != null) ? _db.FavoriteMissions.Any(e => e.MissionId == mission.MissionId && e.UserId == id) : false,
                        //userApplied = (user != null) ? _db.MissionApplications.Any(e => e.MissionId == mission.MissionId && e.UserId == id && int.Parse(e.ApprovalStatus) == 1) : false,
                        ImgUrl = "~/images/Grow-Trees-On-the-path-to-environment-sustainability-3.png",
                        StartDateEndDate = "From " + startDateNtime[0] + " until " + endDateNtime[0],
                        NoOfSeatsLeft = 10,
                        Deadline = endDateNtime[0],
                        createdAt = (DateTime)mission.CreatedAt
                    });
                    switch (sortOrder)
                    {
                        case "newest":
                            missionsVMList = missionsVMList.OrderByDescending(e => e.StartDate).ToList();
                            break;
                        case "oldest":
                            missionsVMList = missionsVMList.OrderBy(e => e.StartDate).ToList();
                            break;
                        case "lowest":
                            missionsVMList = missionsVMList.OrderBy(e => e.NoOfSeatsLeft).ToList();
                            break;
                        case "highest":
                            missionsVMList = missionsVMList.OrderByDescending(e => e.NoOfSeatsLeft).ToList();
                            break;
                        case "favourites":
                            missionsVMList = missionsVMList.Where(e => e.isFavrouite != false).ToList();
                            break;
                        case "deadline":
                            missionsVMList = missionsVMList.OrderBy(e => e.Deadline).ToList();
                            break;
                        default:
                            missionsVMList = missionsVMList;
                            break;
                    }
                }

                if (missions.Count() == 0)
                {
                    return RedirectToAction("Nomission", "Home");
                }

                int totalMissions = FinalMissionsList.Count();
                ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
                ViewBag.CurrentPage = pageIndex ?? 0;

                ViewBag.NoOfMissions = FinalMissionsList.Count();
                ViewBag.missions = missionsVMList.Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                var missions = Missions.ToList();
                foreach (var mission in missions)
                {
                    City city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                    MissionTheme theme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                    string[] startDateNtime = mission.StartDate.ToString().Split(' ');
                    string[] endDateNtime = mission.EndDate.ToString().Split(' ');
                    var ratings = _db.MissionRatings.Where(e => e.MissionId == mission.MissionId).ToList();
                    var rating = 0;
                    var sum = 0;
                    foreach (var entry in ratings)
                    {
                        sum = sum + int.Parse(entry.Rating);

                    }
                    //rating = sum / ratings.Count;

                    missionsVMList.Add(new MissionViewModel
                    {
                        MissionId = mission.MissionId,
                        Title = mission.Title,
                        Description = mission.Description,
                        City = city.Name,
                        Organization = mission.OrganizationName,
                        Theme = theme.Title,
                        //Rating = rating,
                        StartDate = (DateTime)mission.StartDate,
                        EndDate = (DateTime)mission.EndDate,
                        missionType = mission.MissionType,
                        isFavrouite = (user != null) ? _db.FavoriteMissions.Any(e => e.MissionId == mission.MissionId && e.UserId == id) : false,
                        //userApplied = (user != null) ? _db.MissionApplications.Any(e => e.MissionId == mission.MissionId && e.UserId == id && e.ApprovalStatus == '1') : false,
                        ImgUrl = "~/images/Grow-Trees-On-the-path-to-environment-sustainability-3.png",
                        StartDateEndDate = "From " + startDateNtime[0] + " until " + endDateNtime[0],
                        NoOfSeatsLeft = 10,
                        Deadline = endDateNtime[0],
                        createdAt = (DateTime)mission.CreatedAt
                    });

                }
                switch (sortOrder)
                {
                    case "newest":
                        missionsVMList = missionsVMList.OrderByDescending(e => e.StartDate).ToList();
                        break;
                    case "oldest":
                        missionsVMList = missionsVMList.OrderBy(e => e.StartDate).ToList();
                        break;
                    case "lowest":
                        missionsVMList = missionsVMList.OrderBy(e => e.NoOfSeatsLeft).ToList();
                        break;
                    case "highest":
                        missionsVMList = missionsVMList.OrderByDescending(e => e.NoOfSeatsLeft).ToList();
                        break;
                    case "favourites":
                        missionsVMList = missionsVMList.Where(e => e.isFavrouite != false).ToList();
                        break;
                    case "deadline":
                        missionsVMList = missionsVMList.OrderBy(e => e.Deadline).ToList();
                        break;
                    default:
                        missionsVMList = missionsVMList;
                        break;
                }

                if (missions.Count() == 0)
                {
                    return RedirectToAction("Nomission", "Home");
                }

                int totalMissions = Missions.Count();
                ViewBag.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
                ViewBag.CurrentPage = pageIndex ?? 0;

                ViewBag.NoOfMissions = Missions.Count();
                ViewBag.missions = missionsVMList.Skip(skip).Take(pageSize).ToList();
            }

            var MissionApp = _db.MissionApplications.ToList();

            // Get the current URL
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

            return View();
        }

        

        // TESTING ENDPOINTS
        public IActionResult Test(string? searchTerm)
        {
            List<City> Cities = _db.Cities.ToList();
            ViewBag.Cities = Cities;
            List<Country> Country = _db.Countries.ToList();
            ViewBag.Country = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;
            ViewBag.searchTerm = searchTerm;
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
                              missionType = m.MissionType,
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

            var mission = from m in _db.Missions.Where(x => x.Title.Contains(searchTerm))
                          select new
                          {
                              title = m.Title,
                              desc = m.Description,
                              startdate = m.StartDate,
                              enddate = m.EndDate,
                              missionType = m.MissionType,
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

        [Route("home/api/sort")]
        [HttpGet]
        public IActionResult TestSort(string? sortBy, int? pageIndex)
        {

            var mission = from m in _db.Missions
                          select new
                          {
                              title = m.Title,
                              desc = m.Description,
                              startdate = m.StartDate,
                              enddate = m.EndDate,
                              missionType = m.MissionType,
                              city = m.City,
                              theme = m.Theme
                          };

            if (sortBy == "Newest")
            {
                mission = from m in _db.Missions.OrderByDescending(o => o.StartDate)
                          select new
                            {
                              title = m.Title,
                              desc = m.Description,
                              startdate = m.StartDate,
                              enddate = m.EndDate,
                              missionType = m.MissionType,
                              city = m.City,
                              theme = m.Theme
                          };
            }
            if (sortBy == "Oldest")
            {
                mission = from m in _db.Missions.OrderBy(o => o.StartDate)
                          select new
                          {
                              title = m.Title,
                              desc = m.Description,
                              startdate = m.StartDate,
                              enddate = m.EndDate,
                              missionType = m.MissionType,
                              city = m.City,
                              theme = m.Theme
                          };
            }
            if (sortBy == "Deadline")
            {
                mission = from m in _db.Missions.OrderBy(o => o.EndDate)
                          select new
                          {
                              title = m.Title,
                              desc = m.Description,
                              startdate = m.StartDate,
                              enddate = m.EndDate,
                              missionType = m.MissionType,
                              city = m.City,
                              theme = m.Theme
                          };
            }



            int pageSize = 9;
            int skip = (pageIndex ?? 0) * pageSize;
            var Missions = mission.Skip(skip).Take(pageSize).ToList();
            int totalMissions = mission.Count();
            int TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            int CurrentPage = pageIndex ?? 0;

            return Json(new { Missions, totalMissions, TotalPages, CurrentPage, sortBy });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NoMission()
        {
            String? userId = HttpContext.Session.GetString("userEmail");

            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            ViewBag.user = user;

            List<City> Cities = _db.Cities.ToList();
            ViewBag.cities = Cities;
            List<Country> Country = _db.Countries.ToList();
            ViewBag.countries = Country;
            List<MissionTheme> Themes = _db.MissionThemes.ToList();
            ViewBag.Themes = Themes;
            ViewBag.cityElements = Cities;
            ViewBag.themeElements = Themes;
            ViewBag.countryElements = Country;
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