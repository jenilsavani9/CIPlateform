using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class AdminMissionModel
    {
        public long id { get; set; }
        public long themeId { get; set; }
        public string? title { get; set; }
        public string? organization { get; set; }
        public string? description { get; set; }
        public string? organizationDetails { get; set; }
        public string? availability { get; set; }
        public string? missionType { get; set; }
        public string? status { get; set; }
        public List<string>? images { get; set; }
        public List<string>? docs { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? deadline { get; set; }
        public int countryId { get; set; }
        public int cityId { get; set; }
        public int? seats { get; set; }
        public string? missionObjective { get; set; }
        public string? missionTarget { get; set; }

    }
}
