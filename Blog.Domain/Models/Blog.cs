using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Domain.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required]
        [MaxLength(200)]
        public string BlogTitle { get; set; } = string.Empty;

        [Required]
        public string BlogContent { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

    }
}
