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
                //user.city = TempUserData.UserId;
                //user.country = TempUserData.UserId;
                user.profileText = TempUserData.ProfileText;
                user.linkedInUrl = TempUserData.LinkedInUrl;
                user.title = TempUserData.Title;
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
                _db.SaveChanges();
            }
        }
    }
}
