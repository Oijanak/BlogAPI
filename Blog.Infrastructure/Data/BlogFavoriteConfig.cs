using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class BlogFavoriteConfig:IEntityTypeConfiguration<BlogFavorite>
{
    
    public void Configure(EntityTypeBuilder<BlogFavorite> builder)
    {
        builder.HasKey(f => new { f.UserId, f.BlogId });
        builder.HasOne(f => f.User)
            .WithMany(u => u.FavoriteBlogs)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(f => f.Blog)
            .WithMany(b => b.FavoritedBy)
            .HasForeignKey(f => f.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}