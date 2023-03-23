using CI.Entities.Models;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface IUserRepository
    {
        public List<User> Recommend(String? userMail);

        public User? GetUser(String? userMail);

        public bool SaveUser(User user);

        public bool SendMail(User user);

        public PasswordReset? GetResetPassword(string? email, string? token);

        public bool PostResetPassword(ResetPassModel user);
    }
}
