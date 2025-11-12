using System.Text.Json.Serialization;

namespace BlogApi.Application.DTOs;

public class CommentDtos
{
    public Guid CommentId { get; set; }
    public string Content { get; set; }
    public UserDtos User { get; set; }
    
    public DateTime CreatedAt { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? UpdatedAt { get; set; }
    
    public int LikesCount { get; set; }
    
    public int DislikesCount { get; set; }
    
    public bool? CurrentUserReaction { get; set; } 
    public List<CommentDtos>? Replies { get; set; } = new();
}

public class ReactToCommentDto
{
    public Guid CommentId { get; set; }
    public bool IsLike { get; set; } 
}