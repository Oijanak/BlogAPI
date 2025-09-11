using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Domain.Models
{
    public class Blog
    {
        public Guid BlogId { get; init; }=Guid.NewGuid();
        
        public string BlogTitle { get; set; } = string.Empty;
        
        public string BlogContent { get; set; } = string.Empty;

        public DateTime CreatedAt{ get; init; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
        
        public Author Author { get; set; } = null!;
        
        public Guid AuthorId { get; set; }
        

    }
}
