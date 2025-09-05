using System.ComponentModel.DataAnnotations;
using BlogApi.Domain.Models;

namespace BlogApi.Application.DTOs;

public class BlogDTO
{
    public int BlogId { get; set; }
    public string BlogTitle { get; set; } = string.Empty;
    public string BlogContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt{ get; set; }

    public Author Author { get; set; } = null!;

}

public class CreateBlogRequest
{
    public string BlogTitle { get; set; } = string.Empty;
    public string BlogContent { get; set; } = string.Empty;
    public int AuthorId{get;set;}
}

public class UpdateBlogRequest
{
    public int AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
}