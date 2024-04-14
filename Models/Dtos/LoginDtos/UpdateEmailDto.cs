using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystem.Models.Dtos.LoginDtos
{
    public class UpdateEmailDto
    {
        [Column("email_adress")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Email needs to be between 2 and 50 characters")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }
    }
}