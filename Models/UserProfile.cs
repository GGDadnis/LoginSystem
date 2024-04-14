using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystem.Models
{
    [Table("user_profile")]
    public class UserProfile
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Ulid.NewUlid().ToString();

        [Column("first_name")]
        public required string FirstName { get; set; }

        [Column("last_name")]
        public required string LastName { get; set; }

        [Column("photo")]
        public string? Photo { get; set; }

        [Column("login_id")]
        [ForeignKey("Login")]
        public required string LoginId { get; set; }

        public required virtual Login Login { get; set; }

    }
}