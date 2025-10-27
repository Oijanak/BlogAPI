using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class CommentReactionConfig:IEntityTypeConfiguration<CommentReaction>
{
    public void Configure(EntityTypeBuilder<CommentReaction> builder)
    {
        builder
            .HasKey(cr => cr.ReactionId);

        builder
            .HasOne(cr => cr.Comment)
            .WithMany(c => c.Reactions)
            .HasForeignKey(cr => cr.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cr => new { cr.CommentId, cr.UserId })
            .IsUnique(); 

    }
}