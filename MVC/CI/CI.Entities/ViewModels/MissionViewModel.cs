namespace CI.Entities.ViewModels
{
    public class MissionViewModel
    {
        public long MissionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Organization { get; set; }
        public int Rating { get; set; }
        public string ImgUrl { get; set; }
        public string Theme { get; set; }
        public string missionType { get; set; }
        public bool isFavrouite { get; set; }
        public bool userApplied { get; set; }
        public string City { get; set; }
        public string StartDateEndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfSeatsLeft { get; set; }
        public string Deadline { get; set; }
        public DateTime createdAt { get; set; }
        public string Action { get; set; }
    }
}
