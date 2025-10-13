using System.Text.Json.Serialization;

namespace BlogApi.Application.DTOs;

public class CommentDto
{
    public Guid CommentId { get; set; }
    public string Content { get; set; }
    public UserDto User { get; set; }
    public DateTime CreatedAt { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? UpdatedAt { get; set; }
}