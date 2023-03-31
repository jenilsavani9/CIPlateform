using CI.Entities.Models;

namespace CI.Entities.ViewModels
{
    public class StoryDetailsModel
    {
        public string? whyIVolunteer { get; set; }

        public string? missionTitle { get; set; }

        public long? missionId { get; set; }

        public string? storyDetails { get; set; }

        public string? userName { get; set; }

        public string? avatar { get; set; }

        public long? views { get; set; }

        public List<StoryMedium>? storyMedia { get; set; }
    }
}
