using System.ComponentModel.DataAnnotations;

namespace BlogApi.Application.DTOs;

public class BlogDTO
{
    public int BlogId { get; set; }
    public string BlogTitle { get; set; } = string.Empty;
    public string BlogContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt{ get; set; }

}

public class CreateBlogRequest
{
    [Required]
    public string BlogTitle { get; set; } = string.Empty;
    [Required]
    public string BlogContent { get; set; } = string.Empty;
}

public class UpdateBlogRequest
{
    public string? BlogTitle { get; set; }
    public string? BlogContent { get; set; }
}