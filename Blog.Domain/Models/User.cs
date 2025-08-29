using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace BlogApi.Domain.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; }=string.Empty;

        
        [NotMapped]
        public string Password
        {
            set {
                 if (!string.IsNullOrWhiteSpace(value))
                {
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(value);
                }
            }
    
        }

        public ICollection<Blog> Blogs { get; } = new List<Blog>();

        

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, PasswordHash);
        }


    }
}
