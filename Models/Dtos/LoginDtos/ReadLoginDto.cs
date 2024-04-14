using LoginSystem.Models.Dtos.UserProfileDtos;

namespace LoginSystem.Models.Dtos.LoginDtos
{
    public class ReadLoginDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public bool EmailValidation { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReadUserProfileDto? UserProfile { get; set; }
    }
}