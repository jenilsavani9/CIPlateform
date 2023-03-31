using CI.Entities.Models;
using CI.Entities.ViewModels;

namespace CI.Repository.Interface
{
    public interface IVolunteerRepository
    {
        public User? GetUser(String? userMail);

        public List<Mission>? GetMissions(int? id);

        public List<City>? GetCitys(long? id);

        public List<Country>? GetCountrys(long? id);

        public List<MissionTheme>? GetThemes(long? id);

        public List<Mission>? RelatedMissions(long? CityId, long? CountryId, long? ThemeId);

        public bool AddToFavorite(long? userId, int? missionId);

        public List<FavoriteMission>? CheckAddToFavorite(long userId, int id);

        public string? Rating(long userId, int rate, int missionId);

        public List<MissionRating> CheckRating(long userId, int missionId);

        public List<MissionRating> RatingGroup(int missionId);

        public bool Recommand(int id, int missionId);

        public Mission? Organization(int missionId);

        public List<CommentsModel> GetComments(int missionId);

        public bool AddComments(long userId, int missionId, string? getTextarea);

        public MissionApplication GetApplyMission(long userId, int missionId);

        public bool ApplyMission(long userId, int missionId);

        public List<RecentVolunteerModel> GetVolunteers(int missionId, string page);

        public List<MissionDocument> GetMissionDocument(int missionId);

        public List<MissionSkillModel> GetMissionSkills(int missionId);

        public List<MissionAvailableModel> GetMissionDays(int missionId);

    }
}
