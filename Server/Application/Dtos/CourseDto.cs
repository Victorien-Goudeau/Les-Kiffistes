namespace Application.Dtos
{
    public class CourseDto
    {
        public required string Id { get; set; }
        public required Status Status { get; set; } // "In Progress", "Failed" or "Finished"
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public byte[]? FileContent { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}