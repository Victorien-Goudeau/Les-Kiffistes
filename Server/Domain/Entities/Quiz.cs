using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Quiz
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string CourseId { get; set; }
        public string? AIModuleId { get; set; } // If it's an module quiz
        public AIModule? AIModule { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }
        public string? Title { get; set; }
        public Status? Status { get; set; } // "Pending", "In Progress", "Failed" or "Succeeded"
        public float? Result { get; set; }
        public List<Question> Questions { get; set; } = new();
    }
}