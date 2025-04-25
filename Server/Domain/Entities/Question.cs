using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Question
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string QuizId { get; set; }
        public required Quiz Quiz { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }
        public required string Type { get; set; } // "Checkbox", "Multiple Choice" or "Open"
        public List<string> CorrectAnswers { get; set; } = new();
        public List<string> Choices { get; set; } = new();
    }
}