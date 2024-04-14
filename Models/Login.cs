using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystem.Models
{
    [Table("login")]
    public class Login
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Ulid.NewUlid().ToString();

        [Column("email_adress")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Email needs to be between 2 and 50 characters")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Column("email_validation")]
        public bool EmailValidation { get; set; }

        [Column("password_hash")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{4,}$", ErrorMessage = "Password must be at least 4 characters long and contain at least one letter(A-Z) and one digit(0-9).")]
        public required string PasswordHash { get; set; }

        [Column("salt")]
        public required string Salt { get; set; }

        [Column("created_at")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.Date;

        public virtual UserProfile? UserProfile { get; set; }
    }
}