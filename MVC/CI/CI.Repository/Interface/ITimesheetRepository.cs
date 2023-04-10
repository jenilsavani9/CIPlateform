using CI.Entities.Models;
using CI.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface ITimesheetRepository
    {
        public User GetUser(string? userEmail);

        public List<TimeSheetModel> GetGoalBasedTimeSheet(string? userEmail);

        public List<TimeSheetModel> GetTimeBasedTimeSheet(string? userEmail);

        public bool DeleteTimesheet(long? id);

        public bool AddTimeSheets(TimeSheetModel model, long userId);

        public bool AddGoalSheets(TimeSheetModel model, long userId);

        public Timesheet GetSingleTimeSheet(int id);
    }
}
