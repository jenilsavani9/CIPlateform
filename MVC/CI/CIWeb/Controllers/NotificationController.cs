using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CIWeb.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/api/notification")]
        public JsonResult GetNotification()
        {
            var result = _repository.GetGeneralNotification();
            return Json(new { result });
        }
    }
}
