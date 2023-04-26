using CI.Entities.Models;
using CI.Entities.ViewModels;

namespace CI.Repository.Interface
{
    public interface IUserRepository
    {
        public List<User> Recommend(String? userMail);

        public User? GetUser(String? userMail);

        public User? GetValidUser(string? userMail);

        public bool SaveUser(User user);

        public bool SendMail(User user);

        public PasswordReset? GetResetPassword(string? email, string? token);

        public bool PostResetPassword(ResetPassModel user);
    }
}
