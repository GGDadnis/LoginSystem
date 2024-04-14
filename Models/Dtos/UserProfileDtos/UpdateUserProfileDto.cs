using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystem.Models.Dtos.UserProfileDtos
{
    public class UpdateUserProfileDto
    {
        [Column("first_name")]
        public required string FirstName { get; set; }

        [Column("last_name")]
        public required string LastName { get; set; }

        [Column("photo")]
        public string? Photo { get; set; }
    }
}