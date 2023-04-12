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

        public List<User> GetUsers()
        {
            var users = _db.Users.ToList();
            return users;
        }
    }
}
