namespace BlogApi.Domain.Models;



public class AuthorFollower
{
    public string UserId { get; set; }        
    public User User { get; set; }

    public Guid AuthorId { get; set; }      
    public Author Author { get; set; }

    public DateTime FollowedOn { get; set; } = DateTime.UtcNow;
}
