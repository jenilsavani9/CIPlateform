using CI.Entities.Data;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly CiContext _db;
        private readonly IAdminRepository _repository;

        public AdminController(CiContext db, IAdminRepository repository)
        {
            _db = db;
            _repository = repository;
        }

        public IActionResult Index()
        {
            String? userId = HttpContext.Session.GetString("userEmail");
            var user = _repository.FindUser(userId);
            if(user!=null && user.Admin == 1)
            {
                ViewBag.user = user;
                return View();
            } else
            {
                return RedirectToAction("ErrorNotFound", "Home");
            }
        }

        [HttpGet("api/admin/user")]
        public IActionResult GetUsers()
        {
            var users = _repository.GetUsers();
            return Json(new {users});
        }
    }
}
