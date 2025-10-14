namespace BlogApi.Domain.Models;

public class BlogFavorite
{
    public string UserId { get; set; }
    public User User { get; set; }

    public Guid BlogId { get; set; }
    public Blog Blog { get; set; }

    public DateTime FavoritedOn { get; set; } = DateTime.UtcNow;
}