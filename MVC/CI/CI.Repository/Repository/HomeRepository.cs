using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;

namespace CI.Repository.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly CiContext _db;

        private List<MissionViewModel> missionsVMList = new List<MissionViewModel>();

        private List<Mission> Missions = new List<Mission>();

        private List<Mission> FinalMissionsList = new List<Mission>();

        private List<Skill> skillElements = new List<Skill>();

        public HomeRepository(CiContext db)
        {
            _db = db;
        }

        public List<City> GetCities()
        {
            var cities = _db.Cities.ToList();
            return cities;
        }

        public List<Country> GetCountry()
        {
            var countries = _db.Countries.ToList();
            return countries;
        }

        public List<Mission> GetMissions()
        {
            var missions = _db.Missions.Where(m => m.DeletedAt == null).ToList();
            return missions;
        }

        public List<Skill> GetSkill()
        {
            var skills = _db.Skills.Where(s => s.Status == "1").ToList();
            return skills;
        }

        public List<MissionTheme> GetTheme()
        {
            var themes = _db.MissionThemes.Where(t => t.Status == 1).ToList();
            return themes;
        }

        public User FindUser(string? email)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == email);

            return user!;
        }

        public MissionModel GetFilterMissions(string? userId, string? searchQuery, long[] FCountries, long[] FCities, long[] FThemes, long[] FSkills, int? pageIndex, string sortOrder)
        {

            var user = _db.Users.Where(e => e.Email == userId).SingleOrDefault();

            MissionModel model = new MissionModel();
            Missions = _db.Missions.Where(m => m.DeletedAt == null).ToList();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                Missions = Missions.Where(m => m.Title.ToLower().Contains(searchQuery.ToLower())).ToList();
                model.searchQuery = searchQuery;
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

                // for convert long to string FCountries
                var FC = "";
                foreach (var item in FCountries)
                {
                    FC += item.ToString();
                }
                model.FCountries = FC;
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
                //model.FCities = Request.Query["FCities"];
                var FC = "";
                foreach (var item in FCities)
                {
                    FC += item.ToString();
                }
                model.FCities = FC;
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
                model.themeElements = tempThemeElements;

                //model.FThemes = Request.Query["FThemes"];
                var FC = "";
                foreach (var item in FThemes)
                {
                    FC += item.ToString();
                }
                model.FThemes = FC;
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
                model.skillElements = skillElements;

                //model.FSkills = Request.Query["FSkills"];
                var FC = "";
                foreach (var item in FSkills)
                {
                    FC += item.ToString();
                }
                model.FSkills = FC;
            }

            // pagination
            int pageSize = 9; // change this to your desired page size
            int skip = (pageIndex ?? 0) * pageSize;


            var missions = FinalMissionsList.ToList();
            foreach (var mission in missions)
            {

                var city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                var theme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                string[] startDateNtime = mission.StartDate.ToString()!.Split(' ');
                string[] endDateNtime = mission.EndDate.ToString()!.Split(' ');
                var ratings = _db.MissionRatings.Where(e => e.MissionId == mission.MissionId).ToList();
                var missionURL = _db.MissionMedia.Where(e => e.MissionId == mission.MissionId).FirstOrDefault();
                var goalMission = _db.GoalMissions.FirstOrDefault(m => m.MissionId == mission.MissionId);
                var goalAchive = _db.Timesheets.Where(t => t.MissionId == mission.MissionId).Sum(m => m.Action);
                var sum = 0;
                foreach (var entry in ratings)
                {
                    sum = sum + int.Parse(entry.Rating);

                }

                missionsVMList.Add(new MissionViewModel
                {
                    MissionId = mission.MissionId,
                    Title = mission.Title,
                    Description = mission.Description,
                    City = city?.Name,
                    Organization = mission.OrganizationName,
                    Theme = theme?.Title,
                    StartDate = (DateTime)mission.StartDate!,
                    EndDate = (DateTime)mission.EndDate!,
                    missionType = mission.MissionType,
                    isFavrouite = (user != null) ? _db.FavoriteMissions.Any(e => e.MissionId == mission.MissionId && e.UserId == user.UserId) : false,
                    userApplied = (user != null) ? _db.MissionApplications.Any(e => e.MissionId == mission.MissionId && e.UserId == user.UserId && e.ApprovalStatus == "Approve") : false,
                    ImgUrl = (missionURL?.MediaPath != null) ? missionURL.MediaPath : "404-Page-image.png",
                    StartDateEndDate = "From " + startDateNtime[0] + " until " + endDateNtime[0],
                    NoOfSeatsLeft = (int)mission?.SeatLeft!,
                    Deadline = endDateNtime[0],
                    goalValue = (goalMission != null) ? goalMission.GoalValue : "",
                    goalObjective = (goalMission != null) ? goalMission.GoalObjectiveText : "",
                    goalAchived = (goalMission != null) ? (goalAchive * 100) / Int32.Parse(goalMission.GoalValue) : 0,
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

            int totalMissions = missionsVMList.Count();
            model.TotalPages = (int)Math.Ceiling(totalMissions / (double)pageSize);
            model.CurrentPage = pageIndex ?? 0;

            model.NoOfMissions = missionsVMList.Count();
            model.missionsVMList = missionsVMList.Skip(skip).Take(pageSize).ToList();

            return model;
        }
    }
}
