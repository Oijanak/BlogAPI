using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Domain.Enum;
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
    public UserDto? ApprovedBy { get; set; } 
    
    public ApproveStatus ApproveStatus { get; set; }
    
    public ActiveStatus ActiveStatus { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public  DateTime EndDate { get; set; }
    public UserDto CreatedBy { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserDto? UpdatedBy { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AuthorDto Author { get; set; } 
    
    public List<CategoryDto> Categories { get; set; }

}

public class UpdateBlogRequest
{
    public Guid AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public List<Guid> Categories { get; set; }=new List<Guid>();
    
}
