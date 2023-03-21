using CI.Entities.Data;
using CI.Entities.Models;
using CIWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace CIWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;


        public UserController(ILogger<HomeController> logger, CiContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPassModel obj)
        {

            var user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);
            if (user == null)
            {
                ViewBag.emailnotexist = "Email not exist please register first";
                return View();
            }

            // Generate a password reset token for the user
            var token = Guid.NewGuid().ToString();

            // Store the token in the password resets table with the user's email
            var passwordReset = new PasswordReset
            {
                Email = obj.Email,
                Token = token
            };

            _db.PasswordResets.Add(passwordReset);
            _db.SaveChanges();

            // Send an email with the password reset link to the user's email address
            var resetLink = Url.Action("ResetPassword", "User", new { email = obj.Email, token }, Request.Scheme);
            // Send email to user with reset password link
            // ...
            var fromAddress = new MailAddress("jenilsavani8@gmail.com", "CI Platform");
            var toAddress = new MailAddress(obj.Email);
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

            return RedirectToAction("ForgotPassword", "User");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == email && pr.Token == token);
            if (passwordReset == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Pass the email and token to the view for resetting the password
            var model = new PasswordReset
            {
                Email = email,
                Token = token
            };
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetpassModel obj)
        {

            // Find the user by email
            var user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPassword", "User");
            }

            // Find the password reset record by email and token
            var passwordReset = _db.PasswordResets.FirstOrDefault(pr => pr.Email == obj.Email && pr.Token == obj.Token);
            if (passwordReset == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Update the user's password
            user.Password = obj.Password;
            _db.SaveChanges();

            return RedirectToAction("Login", "User");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User obj, IFormCollection form)
        {

            var ConfirmPassword = form["password1"];
            var Password = form["Password"];
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ViewBag.Message = "Enter Confirm Password";
                return View(obj);
            }
            if (Password != ConfirmPassword)
            {
                ViewBag.Message = "Password Does't Match!";
                return View(obj);
            }
            if (obj.FirstName == null)
            {
                ModelState.AddModelError("FirstName", "FirstName Is required!");
                return View();
            }
            if (obj.LastName == null)
            {
                ModelState.AddModelError("LastName", "LastName Is required!");
                return View();
            }
            if (obj.PhoneNumber == null)
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber Is required!");
                return View();
            }
            if (obj.Email == null)
            {
                ModelState.AddModelError("Email", "Email Is required!");
                return View();
            }
            if (obj.Password == null)
            {
                ModelState.AddModelError("Password", "Password Is required!");
                return View();
            }
            //if (obj.Password != obj.Password1)
            //{
            //    ModelState.AddModelError("Password", "Password does Not Match!");
            //    return View();
            //}

            User user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Email Already Registerd!");
                return View();
            }
            _db.Users.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            HttpContext.Session.Remove("userEmail");
            HttpContext.Session.Remove("firstname");
            return View();
        }

        [HttpPost]
        public IActionResult Login(User obj)
        {

            if (obj.Email == null)
            {
                ModelState.AddModelError("Email", "Email Is required!");
                return View();
            }
            if (obj.Password == null)
            {
                ModelState.AddModelError("Password", "Password Is required!");
                return View();
            }
            User user = _db.Users.FirstOrDefault(u => u.Email == obj.Email);
            var username = obj.Email.Split("@")[0];
            if (user != null)
            {
                if (user.Password == obj.Password)
                {
                    HttpContext.Session.SetString("userEmail", user.Email);
                    HttpContext.Session.SetString("firstname", user.FirstName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Password does not match!");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Email", "User Not found!");
                return View();
            }
            return View();
        }

        

        // get all user for recommand to co-worker
        [HttpGet("/users")]
        public IActionResult Recommend(int id)
        {
            String? userMail = HttpContext.Session.GetString("userEmail");

            var user = _db.Users.Where(u => u.Email != userMail);
            return Json(new { user });
        }
    }
}
