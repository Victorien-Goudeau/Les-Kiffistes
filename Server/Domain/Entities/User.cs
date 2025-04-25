using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class User
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public List<Course> Courses { get; set; } = new();
    }
}
