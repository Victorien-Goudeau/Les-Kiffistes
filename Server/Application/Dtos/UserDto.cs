namespace Application.Dtos
{
    public class UserDto
    {
        public required string id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}