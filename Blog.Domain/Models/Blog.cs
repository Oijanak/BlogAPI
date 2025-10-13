using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApi.Domain.Common;
using BlogApi.Domain.Enum;

namespace BlogApi.Domain.Models
{
    public class Blog:BaseEntity
    {
        public Guid BlogId { get; init; }=Guid.NewGuid();
        
        public string BlogTitle { get; set; } = string.Empty;
        
        public string BlogContent { get; set; } = string.Empty;
        
        public ApproveStatus ApproveStatus { get; set; }=ApproveStatus.Pending;
        
        public ActiveStatus ActiveStatus { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public DateTime CreatedAt{ get; init; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
        
        public string? ApprovedBy { get; set; }
        
        public User? ApprovedByUser { get; set; } 
        
        public Author Author { get; set; } = null!;
        
        public Guid AuthorId { get; set; }
        
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        
        public ICollection<BlogDocument> Documents { get; set; } = new List<BlogDocument>();
        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
