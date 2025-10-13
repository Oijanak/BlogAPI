using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;


namespace BlogApi.Domain.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }=string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        
        public ICollection<AuthorFollower> FollowingAuthors { get; set; } = new List<AuthorFollower>();

    }
}
