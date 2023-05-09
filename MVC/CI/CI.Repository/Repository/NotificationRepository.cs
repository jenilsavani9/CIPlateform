using CI.Entities.Data;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CiContext _db;

        public NotificationRepository(CiContext db)
        {
            _db = db;
        }
        
        public List<NotificationModel> GetGeneralNotification()
        {
            var result = _db.Notifications.ToList();
            List<NotificationModel> notificationList = new List<NotificationModel>();
            foreach(var notification in result)
            {
                notificationList.Add(new NotificationModel
                {
                    Id = notification.NotificationId,
                    Title = notification.Description
                });
            }
            return notificationList;
        }
    }
}
