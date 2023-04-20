using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class MissionApplicationModel
    {
        public string? title { get; set; }
        public long missionId { get; set; }
        public long userId { get; set; }
        public string? username { get; set; }
        public DateTime appliedAt { get; set; }
        public long id { get; set; }
    }
}
