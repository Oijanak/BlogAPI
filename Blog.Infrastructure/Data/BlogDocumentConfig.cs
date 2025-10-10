using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Infrastructure.Data;

public class BlogDocumentConfig:IEntityTypeConfiguration<BlogDocument>
{
    public void Configure(EntityTypeBuilder<BlogDocument> builder)
    {
        builder.HasKey(bd => bd.BlogDocumentId);
        

        builder.Property(bd => bd.DocumentName)
            .IsRequired();

        builder.Property(bd => bd.DocumentPath)
            .IsRequired();

        builder.Property(bd => bd.DocumentType)
            .IsRequired();

        builder.Property(bd => bd.DocumentSize)
            .IsRequired();

     
        builder.HasOne(bd => bd.Blog)
            .WithMany(b => b.Documents)
            .HasForeignKey(bd => bd.BlogId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
    
}