using CI.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface INotificationRepository
    {
        public List<NotificationModel> GetGeneralNotification();
    }
}
