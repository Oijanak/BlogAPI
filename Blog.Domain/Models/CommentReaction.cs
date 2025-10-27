namespace BlogApi.Domain.Models;

public class CommentReaction
{
    public Guid ReactionId { get; set; }
    public Guid CommentId { get; set; }
    public string UserId { get; set; }
    public bool IsLike { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Comment Comment { get; set; }
}