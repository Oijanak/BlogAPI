using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class AuthorConfig:IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey("AuthorId");
        builder.Property("AuthorName").IsRequired();
        builder.Property("Age").IsRequired();
        builder.ToTable(t=>t.HasCheckConstraint("CK_Author_Age", "[Age] >= 0 AND [Age] <= 100"));
        builder.HasIndex(u => u.AuthorEmail).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Author_Email_Format",
            @"AuthorEmail LIKE '_%@_%._%'"
        ));
       
        builder.HasOne(a => a.CreatedByUser)
            .WithMany()
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.UpdatedByUser)
            .WithMany()
            .HasForeignKey(a => a.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}