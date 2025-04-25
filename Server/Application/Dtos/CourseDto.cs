namespace Application.Dtos
{
    public class CourseDto
    {
        public required string Id { get; set; }
        public required string Status { get; set; } // "In Progress", "Failed" or "Finished"
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? FileContent { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}