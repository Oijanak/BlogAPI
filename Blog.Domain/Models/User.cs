using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace BlogApi.Domain.Models
{
    public class User
    {
        public int UserId { get; init; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string PasswordHash { get; set; }=string.Empty;

        
        public string Password
        {
            set {
                 if (!string.IsNullOrWhiteSpace(value))
                {
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(value);
                }
            }
    
        }

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, PasswordHash);
        }


    }
}
