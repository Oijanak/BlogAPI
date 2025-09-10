using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Domain.Models;

namespace BlogApi.Application.DTOs;

public class BlogDTO
{
    public Guid BlogId { get; set; }
    public string BlogTitle { get; set; } = string.Empty;
    public string BlogContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt{ get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AuthorDTO Author { get; set; } 

}

public class CreateBlogRequest
{
    public string BlogTitle { get; set; } = string.Empty;
    public string BlogContent { get; set; } = string.Empty;
    public Guid AuthorId{get;set;}
}

public class UpdateBlogRequest
{
    public Guid AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
}
public class BlogWithAuthorDTO
{
    public Guid BlogId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string AuthorEmail { get; set; }
    public int Age { get; set; }
}