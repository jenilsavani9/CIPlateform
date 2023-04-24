using CI.Entities.Models;
using CI.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface IAdminRepository
    {
        public User FindUser(string? userEmail);

        public List<UserProfileModel> GetUsers();

        public List<MissionViewModel> GetMissions();

        public List<ThemeElementModel> GetMissionThemes();

        public List<Skill> GetMissionSkills();

        public List<MissionApplicationModel> GetMissionApplications();

        public List<AdminStoryModel> GetMissionStory();

        public List<Banner> GetBanner();

        public bool AddUsers(UserProfileModel user);

        public bool EditUsers(UserProfileModel user);

        public UserProfileModel GetUserProfile(long? id);

        public bool DeleteUserProfile(long? id);

        public List<CmsPage> GetCMS();

        public bool AddCms(CMSModel obj);

        public CMSModel GetCMSWithId(long id);

        public bool EditCMS(CMSModel obj);

        public bool DeleteCMS(long? id);

        public bool AddTheme(ThemeElementModel obj);

        public ThemeElementModel GetThemeWithId(long id);   

        public bool EditTheme(ThemeElementModel obj);

        public bool DeleteTheme(long id);

        public bool AddSkill(MissionSkillModel obj);

        public MissionSkillModel GetSkillById(long id);

        public bool EditSkill(MissionSkillModel obj);

        public bool DeleteSkill(long id);

        public bool ApproveMission(long id);

        public bool RejectMission(long id);

        public bool ApproveStory(long id);

        public bool RejectStory(long id);

        public bool DeleteStory(long id);

        public bool AddMission(AdminMissionModel obj);

        public AdminMissionModel LoadMission(long id);

        public bool EditMission(AdminMissionModel obj);

        public bool DeleteMission(long id);

        public List<ThemeElementModel> GetValidMissionThemes();

        public Banner GetBannerById(long id);

        public bool AddBanner(Banner obj);

        public bool DeleteBanner(long id);  
    }
}
