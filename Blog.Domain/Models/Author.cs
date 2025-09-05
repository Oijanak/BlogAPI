namespace BlogApi.Domain.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public int Age{get;set;}
    public ICollection<Blog> Blogs { get; } = new List<Blog>();
}