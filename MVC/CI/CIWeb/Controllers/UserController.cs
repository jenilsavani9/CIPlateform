﻿using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _repository;

        public UserController(ILogger<HomeController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
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

            var user = _repository.GetUser(obj.Email);
            if (user == null)
            {
                ViewBag.emailnotexist = "Email not exist please register first";
                return View();
            }

            _repository.SendMail(user);

            return RedirectToAction("Login", "User");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            var passwordReset = _repository.GetResetPassword(email, token);
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
        public ActionResult ResetPassword(ResetPassModel obj)
        {
            if(ModelState.IsValid)
            {
                // Find the user by email
                var user = _repository.GetUser(obj.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPassword", "User");
                }

                bool IsPasswordReset = _repository.PostResetPassword(obj);
                if (!IsPasswordReset)
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login", "User");
            }
            return View();
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
            if (obj?.PhoneNumber == 0 || obj?.PhoneNumber.ToString().Length < 10 || obj?.PhoneNumber.ToString().Length > 10)
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber is not valid!");
                return View();
            }
            if (obj?.Email == null)
            {
                ModelState.AddModelError("Email", "Email Is required!");
                return View();
            }
            if (obj.Password == null || obj.Password.Length < 8)
            {
                ModelState.AddModelError("Password", "Password is not valid!");
                return View();
            }


            User? user = _repository.GetValidUser(obj.Email);
            if (user != null)
            {
                if(user.Status == "0")
                {
                    ModelState.AddModelError("Email", "Account Suspended!");
                    return View();
                }
                ModelState.AddModelError("Email", "Email Already Registerd!");
                return View();
            }

            _repository.SaveUser(obj);

            return RedirectToAction("Login", "User");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userEmail");
            HttpContext.Session.Remove("firstname");
            //ViewBag.Clear();
            return RedirectToAction("Login", "User");
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

            User? user = _repository.GetUser(obj.Email);

            if (user != null && user.FirstName != null)
            {
                if (BCrypt.Net.BCrypt.Verify(obj.Password, user.Password) && user.Admin == 1)
                {
                    HttpContext.Session.SetString("userEmail", user.Email);
                    return RedirectToAction("Index", "Admin");
                }
                else if (BCrypt.Net.BCrypt.Verify(obj.Password, user.Password))
                {
                    HttpContext.Session.SetString("userEmail", user.Email);
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
        }




        // get all user for recommand to co-worker
        [HttpGet("/users")]
        public IActionResult Recommend(int id)
        {
            String? userMail = HttpContext.Session.GetString("userEmail");

            var user = _repository.Recommend(userMail);
            return Json(new { user });
        }
    }
}
