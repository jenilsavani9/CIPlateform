using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public IActionResult Index()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();
            ViewBag.user = user;

            Missions = _db.Missions.ToList();
            Countries = _db.Countries.ToList();
            Themes = _db.MissionThemes.ToList();
            Skills = _db.Skills.ToList();

            ViewBag.countries = Countries;
            ViewBag.themes = Themes;
            ViewBag.skills = Skills;
            return View();
        }

        [HttpGet("/api/missions")]
        public IActionResult GetMission(string? searchQuery, long[] FCountries, long[] FCities, long[] FThemes, long[] FSkills, int? pageIndex, string sortOrder)
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();

            MissionModel model = new MissionModel();
            Missions = _db.Missions.ToList();
            if (!string.IsNullOrEmpty(searchQuery))
            {

                Missions = Missions.Where(m => m.Title.ToLower().Contains(searchQuery.ToLower())).ToList();
                model.searchQuery = Request.Query["searchQuery"];
            }
            FinalMissionsList = Missions;

            if (FCountries != null && FCountries.Length > 0)
            {
                var element = new List<Mission>();
                var tempFinalList = new List<Mission>();
                var tempCountryElements = new List<CountryElementModel>();
                var filterCityElements = new List<CityElementsModel>();
                foreach (var country in FCountries)
                {
                    element = Missions.Where(m => m.CountryId == country).ToList();
                    tempFinalList.AddRange(element);
                    var countryElement = _db.Countries.Where(m => m.CountryId == country).FirstOrDefault();
                    tempCountryElements.Add(new CountryElementModel
                    {
                        countryId = countryElement?.CountryId,
                        name = countryElement?.Name,
                    });


                    var cities = _db.Cities.Where(m => m.CountryId == country).ToList();
                    foreach (var city in cities)
                    {
                        filterCityElements.Add(new CityElementsModel
                        {
                            cityId = city?.CityId,
                            name = city?.Name,
                        });
                    }

                }
                Missions = element;
                FinalMissionsList = tempFinalList;
                model.countryElements = tempCountryElements;
                model.cities = filterCityElements;
                model.FCountries = Request.Query["FCountries"];
            }

            if (FCities != null && FCities.Length > 0)
            {
                var element = new List<Mission>();
                var tempFinalList = new List<Mission>();
                var tempCityElements = new List<CityElementsModel>();
                foreach (var city in FCities)
                {

                    element = Missions.Where(m => m.CityId == city).ToList();
                    var cityElement = _db.Cities.Where(m => m.CityId == city).FirstOrDefault();
                    tempCityElements.Add(new CityElementsModel
                    {
                        cityId = cityElement?.CityId,
                        name = cityElement?.Name,
                    });

                    tempFinalList.AddRange(element);

                }
                Missions = element;
                FinalMissionsList = tempFinalList;
                model.cityElements = tempCityElements;
                model.FCities = Request.Query["FCities"];
            }

            if (FThemes != null && FThemes.Length > 0)
            {
                var element = new List<Mission>();
                var tempFinalList = new List<Mission>();
                var tempThemeElements = new List<ThemeElementModel>();
                foreach (var theme in FThemes)
                {

                    element = Missions.Where(m => m.ThemeId == theme).ToList();
                    var themeElement = _db.MissionThemes.Where(m => m.MissionThemeId == theme).FirstOrDefault();
                    tempThemeElements.Add(new ThemeElementModel
                    {
                        themeId = themeElement?.MissionThemeId,
                        title = themeElement?.Title
                    });
                    tempFinalList.AddRange(element);
                }
                Missions = element;
                FinalMissionsList = tempFinalList;
                model.FThemes = Request.Query["FThemes"];
                model.themeElements = tempThemeElements;
            }

            if (FSkills != null && FSkills.Length > 0)
            {
                var element = new List<Mission>();
                var tempFinalList = new List<Mission>();
                foreach (var skill in FSkills)
                {
                    var tempList = _db.MissionSkills.Where(m => m.SkillId == skill).ToList();
                    foreach (var skillMission in tempList)
                    {
                        element.AddRange(Missions.Where(m => m.MissionId == skillMission.MissionId).ToList());
                    }
                    Missions = element;
                    var skillElement = _db.Skills.Where(m => m.SkillId == skill).ToList();
                    skillElements.AddRange(skillElement);
                    tempFinalList.AddRange(Missions);
                }
                FinalMissionsList = tempFinalList;
                model.FSkills = Request.Query["FSkills"];
                model.skillElements = skillElements;
            }

            // pagination
            int pageSize = 9; // change this to your desired page size
            int skip = (pageIndex ?? 0) * pageSize;


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
                    City = city?.Name,
                    Organization = mission.OrganizationName,
                    Theme = theme?.Title,
                    //Rating = rating,
                    StartDate = (DateTime)mission.StartDate,
                    EndDate = (DateTime)mission.EndDate,
                    missionType = mission.MissionType,
                    isFavrouite = (user != null) ? _db.FavoriteMissions.Any(e => e.MissionId == mission.MissionId && e.UserId == user.UserId) : false,
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

                }
            }

            int totalMissions = FinalMissionsList.Count();
            model.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            model.CurrentPage = pageIndex ?? 0;

            model.NoOfMissions = FinalMissionsList.Count();
            model.missionsVMList = missionsVMList.Skip(skip).Take(pageSize).ToList();

            return Ok(model);
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