using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class TimeSheetModel
    {

        public long timesheetId { get; set; }
        public long? missionId { get; set; }
        public long userId { get; set; }
        public TimeSpan? timesheetTime { get; set; }
        public DateTime dateVolunteered { get; set; }
        public string? notes { get; set; }
        public string? mission { get; set; }
        public int? action { get; set; }
        public int? jenil { get; set; }

    }
}
