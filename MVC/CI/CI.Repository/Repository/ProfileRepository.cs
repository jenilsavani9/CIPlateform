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

        public void GetUserSkills(string? userEmail)
        {
            var TempUserData = _db.Users.FirstOrDefault(x => x.Email == userEmail);
            var skills = from uk in _db.UserSkills
                         join sk in _db.Skills on uk.UserId equals TempUserData.UserId
                         select new
                         {
                             skill = sk
                         };
            //
        }
    }
}
