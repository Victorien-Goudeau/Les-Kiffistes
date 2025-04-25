using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Course
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required User User { get; set; }
        public required string Status { get; set; } // "In Progress" or "Finished"
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<Quiz> Quiz { get; set; } = new();
        public List<AIModule> AIModules { get; set; } = new();
    }
}
