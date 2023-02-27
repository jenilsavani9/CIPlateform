using CIWeb.Data;
using CIWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CiContext _db;


        public HomeController(ILogger<HomeController> logger, CiContext db) 
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(User obj)
        {
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
            //if (obj.Password == obj.Password1)
            //{
            //    ModelState.AddModelError("Password", "Password Is required!");
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
            return RedirectToAction("Index");
        }

        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(User obj)
        {

            if(obj.Email == null)
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
            if (user != null) {
                if (user.Password == obj.Password)
                {
                    return RedirectToAction("Index");
                } else
                {
                    ModelState.AddModelError("Password", "Password does not match!");
                    return View();
                }
            } else
            {
                ModelState.AddModelError("Email", "User Not found!");
                return View();
            }
            return View();
        }

        public IActionResult ErrorNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}