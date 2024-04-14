using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystem.Models.Dtos.LoginDtos
{
    public class AddLoginDto
    {
        [Column("email_adress")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Email needs to be between 2 and 50 characters")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Column("password_hash")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{4,}$", ErrorMessage = "Password must be at least 4 characters long and contain at least one letter and one digit.")]
        public required string PasswordHash { get; set; }
    }
}