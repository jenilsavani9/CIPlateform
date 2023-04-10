using CI.Entities.Models;
using CI.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface IProfileRepository
    {
        public User FindUser(string? email);

        public UserProfileModel GetUserProfile(string? email);

        public void UpdateUserData(UserProfileModel user);

        public List<CountryElementModel> GetCountrys();

        public List<CityElementsModel> GetCountryCity(long countryId);

        public long? GetUserCountry(string? userEmail);

        public List<AddSkillModel> GetUserSkills(string? userEmail);

        public List<AddSkillModel> GetNotUserSkills(string? userEmail);

        public string ChangePassword(string? userEmail, string? oldPassword, string? newPassword);

        public void SaveUserSkills(string? userEmail, List<string> skillsToAdd);

        public bool ContactUs(ContactU obj);
    }
}
