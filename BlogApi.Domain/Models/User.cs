using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace BlogApi.Domain.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        private string _passwordHash = string.Empty;

        [JsonIgnore]
        [Required]
        public string Password
        {
            get => _passwordHash;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
                }
            }
        }

        public ICollection<Blog> Blogs { get; } = new List<Blog>();

        

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, _passwordHash);
        }


    }
}
