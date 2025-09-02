using System;
using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
        .ToTable(t => t.HasCheckConstraint(
            "CK_User_Email_Format",
            @"Email LIKE '_%@_%._%'"
        ));


        modelBuilder.Entity<Blog>()
        .Property(b => b.CreatedAt)
        .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Blog>()
       .Property(b => b.UpdatedAt)
       .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Blog>()
        .HasOne(b => b.User)
        .WithMany(u => u.Blogs)
        .HasForeignKey(b=>b.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
    
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<Blog>()
        .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
    }


}
