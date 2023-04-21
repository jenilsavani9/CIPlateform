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
            if(user == null)
            {
                User tempReturn = new User();
                return tempReturn;
            }
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
                if(mission.Title!=null && mission.Description!=null 
                    && mission.OrganizationName != null
                    && mission.MissionType != null
                    && mission.StartDate != null
                    && mission.EndDate != null
                    && mission.Status != null)
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
            var missionApplication = _db.MissionApplications.Where(ma => ma.ApprovalStatus == "pending").ToList();
            List<MissionApplicationModel> tempMissionApplication = new List<MissionApplicationModel>();
            foreach (var application in missionApplication)
            {
                if(application.Mission.Title != null)
                {
                    tempMissionApplication.Add(new MissionApplicationModel
                    {
                        missionId = application.MissionId,
                        userId = application.UserId,
                        appliedAt = application.CreatedAt,
                        title = application.Mission.Title,
                        username = application.User.FirstName + " " + application.User.LastName,
                        id = application.MissionApplicationId
                    });
                }
                
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

        public List<CmsPage> GetCMS()
        {
            var cms = _db.CmsPages.ToList();
            return cms;
        }

        public List<Banner> GetBanner()
        {
            var banner = _db.Banners.OrderBy(b => b.SortOrder).ToList();
            return banner;
        }

        public bool AddUsers(UserProfileModel user)
        {
            User tempUser = new User();
            tempUser.FirstName = user.firstName;
            tempUser.LastName = user.lastName;
            tempUser.PhoneNumber = user.phoneNumber;
            if(user.email != null)
            {
                tempUser.Email = user.email;
            }
            if (user.password != null)
            {
                tempUser.Password = user.password;
            }
            if (user.avatar != null)
            {
                tempUser.Avatar = user.avatar;
            }
            tempUser.EmployeeId = user.employeeId;
            tempUser.Department = user.department;
            tempUser.CityId = (long)Convert.ToDouble(user.city);
            tempUser.CountryId = (long)Convert.ToDouble(user.country);
            _db.Users.Add(tempUser);
            _db.SaveChanges();
            return true;
        }

        public bool EditUsers(UserProfileModel user)
        {
            var tempUser = _db.Users.Where(u => u.UserId == user.Id).FirstOrDefault();
            if(tempUser != null && user.email != null)
            {
                tempUser.FirstName = user.firstName;
                tempUser.LastName = user.lastName;
                tempUser.PhoneNumber = user.phoneNumber;
                tempUser.Email = user.email;
                if (user.avatar != null)
                {
                    tempUser.Avatar = user.avatar;
                }
                tempUser.EmployeeId = user.employeeId;
                tempUser.Department = user.department;
                tempUser.CityId = (long)Convert.ToDouble(user.city);
                tempUser.CountryId = (long)Convert.ToDouble(user.country);
                tempUser.ProfileText = user.profileText;
                if(user.status != null)
                {
                    tempUser.Status = user.status;
                }
                _db.SaveChanges();
            }
            
            return true;
        }

        public UserProfileModel GetUserProfile(long? id)
        {
            UserProfileModel user = new UserProfileModel();
            var TempUserData = _db.Users.FirstOrDefault(x => x.UserId == id);
            if(TempUserData != null)
            {
                var tempCity = _db.Cities.Where(c => c.CityId == TempUserData.CityId).FirstOrDefault();
                if (TempUserData != null)
                {
                    user.Id = TempUserData.UserId;
                    user.firstName = TempUserData.FirstName;
                    user.lastName = TempUserData.LastName;
                    user.phoneNumber = TempUserData.PhoneNumber;
                    user.avatar = TempUserData.Avatar;
                    user.whyIVolunteer = TempUserData.WhyIVolunteer;
                    user.employeeId = TempUserData.EmployeeId;
                    user.department = TempUserData.Department;
                    user.manager = TempUserData.Manager;
                    user.profileText = TempUserData.ProfileText;
                    user.linkedInUrl = TempUserData.LinkedInUrl;
                    user.title = TempUserData.Title;
                    user.available = TempUserData.Available;
                    user.email = TempUserData.Email;
                    user.countryId = TempUserData.CountryId;
                    user.status = TempUserData.Status;
                    if (tempCity != null)
                    {
                        user.city = tempCity.Name;
                        user.cityId = tempCity.CityId;
                    }

                }
            }
            
            return user;
        }

        public bool DeleteUserProfile(long? id)
        {
            var TempUserData = _db.Users.FirstOrDefault(x => x.UserId == id);
            if(TempUserData != null)
            {
                TempUserData.DeletedAt = DateTime.Now;
                TempUserData.Status = "0";
                _db.SaveChanges();
            }
            return true;
        }

        public bool AddCms(CMSModel obj)
        {
            CmsPage cms = new CmsPage();
            cms.Title = obj.title;
            cms.Description = obj.description;
            if(obj.slug != null)
            {
                cms.Slug = obj.slug;
            }
            cms.Status = obj.status;
            _db.CmsPages.Add(cms);
            _db.SaveChanges();
            return true;
        }

        public CMSModel GetCMSWithId(long id)
        {
            var cms = _db.CmsPages.FirstOrDefault(cms => cms.CmsPageId == id);
            CMSModel cmsModel = new CMSModel();
            if(cms!=null && cms.Title != null && cms.Description != null && cms.Status != null)
            {
                cmsModel.title = cms.Title;
                cmsModel.description = cms.Description;
                cmsModel.slug = cms.Slug;
                cmsModel.status = cms.Status;
            }
            
            return cmsModel;
        }

        public bool EditCMS(CMSModel obj)
        {
            var cms = _db.CmsPages.FirstOrDefault(cms => cms.CmsPageId == obj.id);
            if(cms != null && obj!=null && cms.Title != null && obj.slug != null)
            {
                cms.Title = obj?.title;
                cms.Description = obj?.description;
                if(obj?.slug != null)
                {
                    cms.Slug = obj.slug;
                }
                cms.Status = obj?.status;
                cms.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
            
            return true;
        }

        public bool DeleteCMS(long? id)
        {
            var cms = _db.CmsPages.FirstOrDefault(cms => cms.CmsPageId == id);
            if(cms != null)
            {
                cms.Status = "Decline";
                cms.DeletedAt = DateTime.Now;
                _db.SaveChanges();
            }
            
            return true;
        }

        public bool AddTheme(ThemeElementModel obj)
        {
            MissionTheme theme = new MissionTheme();
            if(obj!=null && obj?.status != null)
            {
                theme.Title = obj?.title;
                if(obj != null)
                {
                    theme.Status = obj.status;
                }
                
                _db.MissionThemes.Add(theme);
                _db.SaveChanges();
            }
            
            return true;
        }

        public ThemeElementModel GetThemeWithId(long id)
        {
            var Theme = _db.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == id);
            ThemeElementModel theme = new ThemeElementModel();
            if(Theme != null)
            {
                theme.title = Theme?.Title;
                theme.themeId = id;
                if(Theme != null)
                {
                    theme.status = Theme.Status;
                }
                
            }
            return theme;
        }

        public bool EditTheme(ThemeElementModel obj)
        {
            var theme = _db.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == obj.themeId);
            if(theme != null)
            {
                theme.Title = obj.title;
                theme.Status = obj.status;
                theme.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteTheme(long id)
        {
            var theme = _db.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == id);
            if(theme != null)
            {
                theme.Status = 2;
                theme.DeletedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AddSkill(MissionSkillModel obj)
        {
            Skill skill = new Skill();
            skill.SkillName = obj.skillName;
            if(obj.status != null)
            {
                skill.Status = obj.status;
            }
            
            _db.Skills.Add(skill);
            _db.SaveChanges();
            return true;
        }

        public MissionSkillModel GetSkillById(long id)
        {
            var skill = _db.Skills.FirstOrDefault(ms => ms.SkillId == id);
            MissionSkillModel answer = new MissionSkillModel();
            if(skill != null)
            {
                answer.skillName = skill.SkillName;
                answer.status = skill.Status;
                answer.skillId = skill.SkillId;
            }
            
            return answer;
        }

        public bool EditSkill(MissionSkillModel obj)
        {
            var skill = _db.Skills.FirstOrDefault(ms => ms.SkillId==obj.skillId);
            if(skill != null)
            {
                skill.SkillName = obj.skillName;
                if(obj.status != null)
                {
                    skill.Status = obj.status;
                }
                
                skill.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            return false;
            
        }

        public bool DeleteSkill(long id)
        {
            var skill = _db.Skills.FirstOrDefault(ms => ms.SkillId == id);
            if(skill!=null)
            {
                skill.Status = "2";
                skill.DeletedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool ApproveMission(long id)
        {
            var application = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == id);
            if (application != null)
            {
                application.ApprovalStatus = "Approve";
                _db.SaveChanges();
                return true;
            }
                
            return false;
        }

        public bool RejectMission(long id)
        {
            var application = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == id);
            if(application != null)
            {
                application.ApprovalStatus = "Reject";
                _db.SaveChanges();
                return true;
            }
            
            return false;
        }

        public bool ApproveStory(long id)
        {
            var story = _db.Stories.FirstOrDefault(s => s.StoryId == id);
            if(story != null)
            {
                story.Status = "approve";
                _db.SaveChanges();
                return true;
            }
            
            return false;
        }

        public bool RejectStory(long id)
        {
            var story = _db.Stories.FirstOrDefault(s => s.StoryId == id);
            if (story != null)
            {
                story.Status = "reject";
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteStory(long id)
        {
            var story = _db.Stories.FirstOrDefault(s => s.StoryId == id);
            if(story != null)
            {
                story.Status = "delete";
                story.DeletedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AddMission(AdminMissionModel obj)
        {
            if (obj == null)
            {
                return false;
            }
            Mission mission = new Mission();
            mission.Title = obj.title;
            mission.Description = obj.description;
            mission.OrganizationName = obj.organization;
            mission.OrganizationDetail = obj.organizationDetails;
            mission.StartDate = obj.startDate;
            mission.EndDate = obj.endDate;
            mission.CountryId = obj.countryId;
            mission.CityId = obj.cityId;
            mission.SeatLeft = obj.seats;
            mission.Availability = obj.availability;
            mission.MissionType = obj.missionType;
            mission.Status = obj.status;
            mission.ThemeId = obj.themeId;
            _db.Missions.Add(mission);
            _db.SaveChanges();

            if(obj.images != null)
            {
                for(var i=0; i<obj.images.Count;i++)
                {
                    var img = obj.images[i];
                    _db.MissionMedia.Add(new MissionMedium { MissionId = mission.MissionId, MediaName = img, MediaType = "png", MediaPath = img });
                    _db.SaveChanges();
                }
            }

            if (obj.docs != null)
            {
                for (var i = 0; i < obj.docs.Count; i++)
                {
                    var doc = obj.docs[i];
                    _db.MissionDocuments.Add(new MissionDocument { MissionId = mission.MissionId, DocumentName = doc.Split(".")[0], DocumentType = doc.Split(".")[1], DocumentPath = doc });
                    _db.SaveChanges();
                }
            }
            return true;
        }

        public AdminMissionModel LoadMission(long id)
        {
            AdminMissionModel model = new AdminMissionModel();
            var tempMission = _db.Missions.FirstOrDefault(m => m.MissionId == id);
            if(tempMission != null)
            {
                model.id = tempMission.MissionId;
                model.title = tempMission.Title;
                model.description = tempMission.Description;
                model.organization = tempMission.OrganizationName;
                model.organizationDetails = tempMission.OrganizationDetail;
                model.startDate = tempMission.StartDate;
                model.endDate = tempMission.EndDate;
                model.seats = tempMission.SeatLeft;
                model.availability = tempMission.Availability;
                model.missionType = tempMission.MissionType;
                model.status = tempMission.Status;
                model.availability = tempMission.Availability;
                if(tempMission.ThemeId != null 
                    && tempMission.CountryId != null
                    && tempMission.CityId != null)
                {
                    model.themeId = (long)tempMission.ThemeId;
                    model.countryId = (int)tempMission.CountryId;
                    model.cityId = (int)tempMission.CityId;
                }
                
                
            }

            // load mission images
            var images = _db.MissionMedia.Where(m => m.MissionId == id).ToList();
            List<string> files = new List<string>();
            foreach (var image in images)
            {
                if(image.MediaPath != null)
                {
                    files.Add(image.MediaPath);
                }
                
            }
            model.images = files;

            // load mission docs
            var docs = _db.MissionDocuments.Where(md => md.MissionId == id).ToList();
            List<string> Docfiles = new List<string>();
            foreach(var doc in docs)
            {
                if(doc.DocumentPath != null)
                {
                    Docfiles.Add(doc.DocumentPath);
                }
                
            }
            model.docs = Docfiles;

            return model;
        }

        public bool EditMission(AdminMissionModel obj)
        {
            if (obj == null)
            {
                return false;
            }
            var mission = _db.Missions.FirstOrDefault(m => m.MissionId == obj.id);
            if(mission != null)
            {
                mission.Title = obj.title;
                mission.Description = obj.description;
                mission.OrganizationName = obj.organization;
                mission.OrganizationDetail = obj.organizationDetails;
                mission.StartDate = obj.startDate;
                mission.EndDate = obj.endDate;
                mission.CountryId = obj.countryId;
                mission.CityId = obj.cityId;
                mission.SeatLeft = obj.seats;
                mission.Availability = obj.availability;
                mission.MissionType = obj.missionType;
                mission.Status = obj.status;
                mission.ThemeId = obj.themeId;
                mission.UpdatedAt = DateTime.Now;
                _db.SaveChanges();


                if (obj.images != null)
                {
                    var images = _db.MissionMedia.Where(mm => mm.MissionId == mission.MissionId).ToList();
                    foreach (var image in images)
                    {
                        _db.MissionMedia.Remove(image);
                    }
                    for (var i = 0; i < obj.images.Count; i++)
                    {
                        var img = obj.images[i];
                        _db.MissionMedia.Add(new MissionMedium { MissionId = mission.MissionId, MediaName = img, MediaType = "png", MediaPath = img });
                        _db.SaveChanges();
                    }
                }

                if (obj.docs != null)
                {
                    var docs = _db.MissionDocuments.Where(md => md.MissionId == mission.MissionId).ToList();
                    foreach (var doc in docs)
                    {
                        _db.MissionDocuments.Remove(doc);
                    }
                    for (var i = 0; i < obj.docs.Count; i++)
                    {
                        var doc = obj.docs[i];
                        _db.MissionDocuments.Add(new MissionDocument { MissionId = mission.MissionId, DocumentName = doc, DocumentType = "pdf", DocumentPath = doc });
                        _db.SaveChanges();
                    }
                }
            } 
            
            return true;
        }

        public bool DeleteMission(long id)
        { 
            if(id == 0)
            {
                return false;
            }
            var mission = _db.Missions.FirstOrDefault(m => m.MissionId==id);    
            if(mission != null)
            {
                mission.Status = "0";
                mission.DeletedAt = DateTime.Now;
                _db.SaveChanges();
            }
            return true;
        }

        public List<ThemeElementModel> GetValidMissionThemes()
        {
            var missionthemes = _db.MissionThemes.Where(t => t.Status == 1).ToList();
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

        // banner management
        public Banner GetBannerById(long id)
        {
            var banner = _db.Banners.FirstOrDefault(b => b.BannerId == id);
            return banner;
        }

        public bool AddBanner(Banner obj)
        {
            var findBanner = _db.Banners.FirstOrDefault(b => b.BannerId==obj.BannerId);
            if(findBanner != null)
            {
                findBanner.Image = obj.Image;
                findBanner.Text = obj.Text;
                findBanner.SortOrder = obj.SortOrder;
                _db.SaveChanges();
                return true;
            } 
            else
            {
                _db.Banners.Add(new Banner { Image = obj.Image, Text = obj.Text, SortOrder = obj.SortOrder });
                _db.SaveChanges();
                return true;
            }
            
        }
    }
}
