using BlogApi.Domain.Common;

namespace BlogApi.Domain.Models
{

    public class Category : BaseEntity
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}