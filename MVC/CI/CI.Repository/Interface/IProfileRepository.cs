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
    }
}
