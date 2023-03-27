using CI.Entities.Models;

namespace CI.Entities.ViewModels
{
    public class StoryModel
    {
        public User? user { get; set; }

        public Story? story { get; set; }

        public Mission? mission { get; set; }

        public MissionTheme? theme { get; set; }
    }
}
