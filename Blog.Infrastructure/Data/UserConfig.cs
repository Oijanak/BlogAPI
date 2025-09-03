using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey("UserId");
        builder.Property("Email").IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_User_Email_Format",
            @"Email LIKE '_%@_%._%'"
        ));
        builder.Ignore("Password");
        builder.Property("PasswordHash").IsRequired();
    }
}
