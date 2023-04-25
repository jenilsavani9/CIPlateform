using CI.Entities.Models;
using CI.Entities.ViewModels;

namespace CI.Repository.Interface
{
    public interface IHomeRepository
    {
        public List<City> GetCities();

        public List<Mission> GetMissions();

        public List<Country> GetCountry();

        public List<MissionTheme> GetTheme();

        public List<Skill> GetSkill();

        public User FindUser(string? email);

        public MissionModel GetFilterMissions(string? userId, string? searchQuery, long[] FCountries, long[] FCities, long[] FThemes, long[] FSkills, int? pageIndex, string sortOrder);
    }
}
