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

        public bool AddUsers(UserProfileModel user);
    }
}
