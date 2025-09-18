using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class BlogConfig:IEntityTypeConfiguration<Blog>
{ 
   void IEntityTypeConfiguration<Blog>.Configure(EntityTypeBuilder<Blog> builder)
   
   {
      builder.HasKey("BlogId");
      builder.Property("BlogId").IsRequired();
      builder.Property("BlogTitle").IsRequired().HasMaxLength(200);
      builder.Property("BlogContent").IsRequired();
      builder.HasOne<Author>(b=>b.Author).WithMany(u=>u.Blogs).HasForeignKey(b=>b.AuthorId).OnDelete(DeleteBehavior.Cascade);
      builder.Property(b => b.CreatedAt)
         .HasDefaultValueSql("GETUTCDATE()");

      builder
         .Property(b => b.UpdatedAt)
         .HasDefaultValueSql("GETUTCDATE()");
      
      builder.HasOne(b => b.CreatedByUser)
         .WithMany()
         .HasForeignKey(b => b.CreatedBy)
         .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(b => b.UpdatedByUser)
         .WithMany()
         .HasForeignKey(b => b.UpdatedBy)
         .OnDelete(DeleteBehavior.Restrict);
   }
    
}