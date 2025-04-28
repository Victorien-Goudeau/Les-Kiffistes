using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class AIModule
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string CourseId { get; set; }
        public required Course Course { get; set; }
        public required string QuizId { get; set; }
        public required Quiz Quiz { get; set; }
        public required Status Status { get; set; } // "In Progress" or "Finished"
        public required int Level { get; set; } // Order in the course
        public DateTimeOffset CreatedAt { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}