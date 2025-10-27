namespace BlogApi.Domain.Models;

public class Comment
{
    public Guid CommentId { get; set; } = Guid.NewGuid();

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Guid BlogId { get; set; }

    public string UserId { get; set; }

    public Blog Blog { get; set; } = null!;

    public User User { get; set; } = null!;
    
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    
    public ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();

}