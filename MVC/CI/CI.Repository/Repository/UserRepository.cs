using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using System.Net;
using System.Net.Mail;

namespace CI.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CiContext _db;

        public UserRepository(CiContext db)
        {
            _db = db;
        }

        public PasswordReset? GetResetPassword(string? email, string? token)
        {
            var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == email && pr.Token == token);
            return passwordReset;
        }

        public User? GetUser(string? userMail)
        {
            var user = _db.Users.Where(u => u.Email == userMail && u.Status == "1").FirstOrDefault();
            return user;
        }

        public User? GetValidUser(string? userMail)
        {
            var user = _db.Users.Where(u => u.Email == userMail).FirstOrDefault();
            return user;
        }

        public bool PostResetPassword(ResetPassModel user)
        {
            // Find the password reset record by email and token
            var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == user.Email && pr.Token == user.Token);
            if (passwordReset == null)
            {
                return false;
            }
            else
            {
                // Update the user's password
                var TempUser = _db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                if(TempUser != null && user.Password != null)
                {
                    TempUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<User> Recommend(String? userMail)
        {
            var user = _db.Users.Where(u => u.Email != userMail).ToList();
            return user;
        }

        public bool SaveUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public bool SendMail(User user)
        {
            // Generate a password reset token for the user
            var token = Guid.NewGuid().ToString();

            // Store the token in the password resets table with the user's email
            var passwordReset = new PasswordReset
            {
                Email = user.Email,
                Token = token
            };

            _db.PasswordResets.Add(passwordReset);
            _db.SaveChanges();

            var resetLink = "https://localhost:44398/user/resetpassword?email=" + user.Email + "&token=" + token;
            // Send an email with the password reset link to the user's email address
            //var resetLink = Url.Action("ResetPassword", "User", new { email = user.Email, token }, Request.Scheme);
            // Send email to user with reset password link
            // ...
            var fromAddress = new MailAddress("jenilsavani8@gmail.com", "CI Platform");
            var toAddress = new MailAddress(user.Email);
            var subject = "Password reset request";
            var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("jenilsavani8@gmail.com", "bwgnmdxyggqrylsu"),
                EnableSsl = true
            };
            smtpClient.Send(message);

            return true;
        }
    }
}
