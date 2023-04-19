namespace CI.Entities.ViewModels
{
    public class CommentsModel
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? commentText { get; set; }
        public DateTime? createdAt { get; set; }
        public string? avatar { get; set; }
    }
}
