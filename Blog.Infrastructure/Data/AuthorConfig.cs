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
    }
}