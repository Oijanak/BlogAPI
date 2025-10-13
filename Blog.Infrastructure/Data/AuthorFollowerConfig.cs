using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class AuthorFollowerConfig:IEntityTypeConfiguration<AuthorFollower>
{
    public void Configure(EntityTypeBuilder<AuthorFollower> builder)
    {
        builder
            .HasKey(af => new { af.UserId, af.AuthorId }); 

        builder
            .HasOne(af => af.User)
            .WithMany(u => u.FollowingAuthors)
            .HasForeignKey(af => af.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(af => af.Author)
            .WithMany(u => u.Followers)
            .HasForeignKey(af => af.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}