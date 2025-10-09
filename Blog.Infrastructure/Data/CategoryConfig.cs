using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class CategoryConfig:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey("CategoryId");
        builder.Property("CategoryName").IsRequired().HasMaxLength(100);
        builder.HasIndex(c=>c.CategoryName).IsUnique();
        builder.HasMany(c=>c.Blogs)
            .WithMany(b=>b.Categories)
            .UsingEntity(t=>t.ToTable("BlogCategories"));
            
    }
}