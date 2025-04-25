using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class CourseDto
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string Status { get; set; } // "In Progress" or "Finished"
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}