using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class TimesheetController : Controller
    {

        private readonly ITimesheetRepository _repository;

        public TimesheetController(ILogger<HomeController> logger, ITimesheetRepository repository)
        {
            _repository = repository;
        }

        // GET: TimesheetController
        public ActionResult Index()
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userEmail);
            ViewBag.user = user;
            return View();
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

        [HttpGet("/api/timesheets/add")]
        public JsonResult AddTimeSheets(TimeSheetModel model)
        {
            String? userEmail = HttpContext.Session.GetString("userEmail");
            var user = _repository.GetUser(userEmail);
            var status = _repository.AddTimeSheets(model, user.UserId);
            return Json(new {  });
        }

        [HttpGet("/api/goalsheets/add")]
        public JsonResult AddGoalSheets(TimeSheetModel model)
        {
            var x = Request;
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
    }
}
