namespace BlogApi.Domain.DTOs;

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
    public string BlogTitle { get; set; } = string.Empty;
    public string BlogContent { get; set; } = string.Empty;
}

public class UpdateBlogRequest
{
    public string? BlogTitle { get; set; }
    public string? BlogContent { get; set; }
}