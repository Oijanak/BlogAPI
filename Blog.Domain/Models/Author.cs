
using BlogApi.Domain.Common;

namespace BlogApi.Domain.Models;

public class Author:BaseEntity
{
    public Guid AuthorId { get; set; }=Guid.NewGuid();
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public int Age{get;set;}
    public ICollection<Blog> Blogs { get; } = new List<Blog>();
    
}