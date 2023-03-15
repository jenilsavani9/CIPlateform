using CI.Entities.Models;

namespace CIWeb.Models
{
    public class VolunteerMissionModel
    {
        public IEnumerable<User>? user { get; set; }

        public IEnumerable<Mission>? Mission { get; set; }

        public IEnumerable<Mission>? relatedMission { get; set; }

        public IEnumerable<City>? City { get; set; }

        public IEnumerable<MissionTheme>? Themes { get; set; }

        public IEnumerable<Country>? Country { get; set; }
    }
}
