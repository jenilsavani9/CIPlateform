using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly CiContext _db;

        public AdminRepository(CiContext db)
        {
            _db = db;
        }

        public User FindUser(string? userEmail)
        {
            var user = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            return user;
        }

        public List<UserProfileModel> GetUsers()
        {
            var users = _db.Users.ToList();
            List<UserProfileModel> tempUsers = new List<UserProfileModel>();
            foreach (var user in users)
            {
                tempUsers.Add(new UserProfileModel
                {
                    Id = user.UserId,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    phoneNumber = user.PhoneNumber,
                    avatar = user.Avatar,
                    employeeId = user.EmployeeId,
                    department = user.Department,
                    manager = user.Manager,
                    profileText = user.ProfileText,
                    linkedInUrl = user.LinkedInUrl,
                    title = user.Title,
                    available = user.Available,
                    email = user.Email,
                    status = user.Status,
                });
            }
            return tempUsers;
        }

        public List<MissionViewModel> GetMissions()
        {
            var missions = _db.Missions.ToList();
            List<MissionViewModel> tempMissions = new List<MissionViewModel>();
            foreach (var mission in missions)
            {
                tempMissions.Add(new MissionViewModel
                {
                    MissionId = mission.MissionId,
                    Title = mission.Title,
                    Description = mission.Description,
                    Organization = mission.OrganizationName,
                    missionType = mission.MissionType,
                    StartDate = (DateTime)mission.StartDate,
                    EndDate = (DateTime)mission.EndDate,
                    Action = mission.Status
                });
            }
            return tempMissions;
        }

        public List<ThemeElementModel> GetMissionThemes()
        {
            var missionthemes = _db.MissionThemes.ToList();
            List<ThemeElementModel> tempMissionThemes = new List<ThemeElementModel>();
            foreach (var theme in missionthemes)
            {
                tempMissionThemes.Add(new ThemeElementModel
                {
                    themeId = theme.MissionThemeId,
                    title = theme.Title,
                    status = theme.Status,
                });
            }
            return tempMissionThemes;
        }

        public List<Skill> GetMissionSkills()
        {
            var missionskills = _db.Skills.ToList();
            return missionskills;
        }

        public List<MissionApplicationModel> GetMissionApplications()
        {
            var missionApplication = _db.MissionApplications.ToList();
            List<MissionApplicationModel> tempMissionApplication = new List<MissionApplicationModel>();
            foreach (var application in missionApplication)
            {
                tempMissionApplication.Add(new MissionApplicationModel
                {
                    missionId = application.MissionId,
                    userId = application.UserId,
                    appliedAt = application.CreatedAt,
                    title = application.Mission.Title,
                    username = application.User.FirstName + " " + application.User.LastName,
                });
            }
            return tempMissionApplication;
        }

        public List<AdminStoryModel> GetMissionStory()
        {
            var tempStory = _db.Stories.ToList();
            List<AdminStoryModel> stories = new List<AdminStoryModel>();
            foreach (var story in tempStory)
            {
                stories.Add(new AdminStoryModel
                {
                    storyId = story.StoryId,
                    storyTitle = story.Title,
                    missionTitle = story.Mission.Title,
                    username = story.User.FirstName + " " + story.User.LastName
                });
            }
            return stories;
        }

        public bool AddUsers(UserProfileModel user)
        {
            User tempUser = new User();
            tempUser.FirstName = user.firstName;
            tempUser.LastName = user.lastName;
            tempUser.PhoneNumber = user.phoneNumber;
            tempUser.Email = user.email;
            tempUser.Password = user.password;
            tempUser.Avatar = user.avatar.ToString().Split('\\')[2];
            tempUser.EmployeeId = user.employeeId;
            tempUser.Department = user.department;
            tempUser.CityId = (long)Convert.ToDouble(user.city);
            tempUser.CountryId = (long)Convert.ToDouble(user.country);
            _db.Users.Add(tempUser);
            _db.SaveChanges();
            return true;
        }
    }
}
