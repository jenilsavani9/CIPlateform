using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _repository;

        public TimesheetController(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        // GET: TimesheetController
        public ActionResult Index()
        {
            try
            {
                String? userEmail = HttpContext.Session.GetString("userEmail");
                var user = _repository.GetUser(userEmail);
                if (user == null)
                {
                    return RedirectToAction("Login", "User");
                }
                ViewBag.user = user;
                return View();
            } 
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
            
        }

        [HttpGet("/api/timesheets")]
        public JsonResult GetTimeSheet()
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var goalSheets = _repository.GetGoalBasedTimeSheet(userEmail);
            var timeSheets = _repository.GetTimeBasedTimeSheet(userEmail);
            return Json(new { goalSheets, timeSheets });
        }

        [HttpGet("/api/timesheets/delete")]
        public JsonResult GetApplyMission(long? id)
        {
            var status = _repository.DeleteTimesheet(id);
            return Json(new { status });
        }

        [HttpPost("/api/timesheets/add")]
        public JsonResult AddTimeSheets(TimeSheetModel model)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userEmail);
            var status = _repository.AddTimeSheets(model, user.UserId);
            return Json(new { status });
        }

        [HttpPost("/api/goalsheets/add")]
        public JsonResult AddGoalSheets(TimeSheetModel model)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userEmail);
            var status = _repository.AddGoalSheets(model, user.UserId);
            return Json(new { });
        }

        [HttpGet("/api/timesheets/{id:int}")]
        public JsonResult GetSingleTimeSheet(int id)
        {
            var result = _repository.GetSingleTimeSheet(id);
            return Json(new { result });
        }

        [HttpPost("/api/timesheets/edit")]
        public JsonResult EditTimeSheets(TimeSheetModel model)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userEmail);
            var status = _repository.EditTimeSheets(model, user.UserId);
            return Json(new { });
        }

        [HttpPost("/api/goalsheets/edit")]
        public JsonResult EditGoalSheets(TimeSheetModel model)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userEmail);
            var status = _repository.EditGoalSheets(model, user.UserId);
            return Json(new { });
        }

        //check date
        [HttpGet("/api/checkDate")]
        public JsonResult CheckDate(string id)
        {
            var result = _repository.CheckDate((long)Convert.ToDouble(id));
            return Json(new { result });
        }
    }
}
