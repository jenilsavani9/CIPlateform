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
    public class ProfileRepository : IProfileRepository
    {
        private readonly CiContext _db;

        public ProfileRepository(CiContext db)
        {
            _db = db;
        }

        public User FindUser(string? email)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public UserProfileModel GetUserProfile(string? email)
        {
            UserProfileModel user = new UserProfileModel();
            var TempUserData = _db.Users.FirstOrDefault(x => x.Email == email);
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
                if(tempCity != null)
                {
                    user.city = tempCity.Name;
                    user.cityId = tempCity.CityId;
                }
                
            }
            return user;
        }

        public void UpdateUserData(UserProfileModel user)
        {
            var TempUser = _db.Users.Find(user.Id);
            if (TempUser != null)
            {
                TempUser.FirstName = user.firstName;
                TempUser.LastName = user.lastName;
                TempUser.WhyIVolunteer = user.whyIVolunteer;
                TempUser.EmployeeId = user.employeeId;
                TempUser.Department = user.department;
                TempUser.ProfileText = user.profileText;
                TempUser.LinkedInUrl = user.linkedInUrl;
                TempUser.Title = user.title;
                TempUser.CityId = user.cityId;
                TempUser.CountryId = user.countryId;
                TempUser.Manager = user.manager;
                TempUser.Avatar = user.avatar;
                TempUser.UpdatedAt = DateTime.Now;
                TempUser.Available = user.available;
                _db.SaveChanges();
            }
        }

        public List<CountryElementModel> GetCountrys()
        {
            List<CountryElementModel> country = new List<CountryElementModel>();
            var tempCountry = _db.Countries.ToList();
            foreach (var countryElement in tempCountry)
            {
                country.Add(new CountryElementModel
                {
                    countryId = countryElement.CountryId,
                    name = countryElement.Name
                });
            }
            return country;
        }

        public List<CityElementsModel> GetCountryCity(long countryId)
        {
            List<CityElementsModel> cities = new List<CityElementsModel>();
            var tempCities = _db.Cities.Where(city => city.CountryId == countryId).ToList();
            foreach (var cityElement in tempCities)
            {
                cities.Add(new CityElementsModel
                {
                    cityId = cityElement.CityId,
                    name = cityElement.Name
                });
            }
            return cities;
        }

        public long? GetUserCountry(string? userEmail)
        {
            var user = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            return user.CountryId;
        }

        public List<AddSkillModel> GetUserSkills(string? userEmail)
        {
            var TempUserData = _db.Users.FirstOrDefault(x => x.Email == userEmail);
            //var userSkills = from s in _db.Skills
            //                 join u in _db.UserSkills on s.SkillId equals u.UserSkillId
            //                 where u.UserId == TempUserData.UserId
            //                 select new
            //                 {
            //                     id = u.SkillId,
            //                     name = u.Skill.SkillName
            //                 };
            var tempskills = _db.UserSkills.Where(us => us.UserId == TempUserData.UserId).ToList();
            List<Skill> userSkills = new List<Skill>();
            foreach (var skill in tempskills)
            {
                var TP = _db.Skills.FirstOrDefault(s => s.SkillId == skill.SkillId);
                userSkills.Add(new Skill
                {
                    SkillId = TP.SkillId,
                    SkillName = TP.SkillName
                    
                });
            }
            List<AddSkillModel> skillList = new List<AddSkillModel>();
            foreach (var skill in userSkills)
            {
                skillList.Add(new AddSkillModel
                {
                    Id = skill.SkillId,
                    Name = skill.SkillName
                });
            }

            return skillList;
            
        }

        public string ChangePassword(string? userEmail, string? oldPassword, string? newPassword)
        {
            var user = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            if(user!=null && newPassword != null && user.Password == oldPassword)
            {
                user.Password = newPassword;
                _db.SaveChanges();
                return "OK";
            } else
            {
                return "Error";
            }
        }

        public List<AddSkillModel> GetNotUserSkills(string? userEmail)
        {
            var skills = _db.Skills.ToList();

            List<AddSkillModel> skillList = new List<AddSkillModel>();
            foreach (var skill in skills)
            {
                skillList.Add(new AddSkillModel
                {
                    Id = skill.SkillId,
                    Name = skill.SkillName
                });
            }

            return skillList;
        }

        public void SaveUserSkills(string? userEmail, List<string> skillsToAdd)
        {
            var user = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            var userSkills = _db.UserSkills.Where(uk => uk.UserId == user.UserId).ToList();
            if(userSkills.Any())
            {
                foreach (var skill in userSkills)
                {
                    _db.Remove(skill);
                }
            }
            foreach (var skill in skillsToAdd)
            {
                var splitSkill = skill.Split(',')[1];
                _db.UserSkills.Add(new UserSkill { SkillId = long.Parse(splitSkill), UserId = user.UserId });
            }
            _db.SaveChanges();
        }

        public bool ContactUs(ContactU obj)
        {
            if (obj == null)
            {
                return false;
            } else
            {
                _db.ContactUs.Add(obj);
                _db.SaveChanges();
                return true;
            }
        }

        public List<CmsPage> PrivacyDetails()
        {
            var result = _db.CmsPages.ToList();
            return result;
        }
    }
}
