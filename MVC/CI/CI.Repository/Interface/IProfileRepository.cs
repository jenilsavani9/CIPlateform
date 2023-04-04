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

        public void GetUserSkills(string? userEmail);
    }
}
