using CI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface IAdminRepository
    {
        public User FindUser(string? userEmail);

        public List<User> GetUsers();
    }
}
