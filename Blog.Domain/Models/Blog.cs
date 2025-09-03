using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Domain.Models
{
    public class Blog
    {
        public int BlogId { get; init; }
        
        public string BlogTitle { get; set; } = string.Empty;
        
        public string BlogContent { get; set; } = string.Empty;

        public DateTime CreatedAt{ get; init; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

        public int UserId { get; init; }
        
        public User User { get; init; } = null!;

    }
}
