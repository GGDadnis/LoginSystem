namespace LoginSystem.Models.Dtos.UserProfileDtos
{
    public class ReadUserProfileDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Photo { get; set; }
    }
}