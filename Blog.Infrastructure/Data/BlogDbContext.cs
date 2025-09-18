using System;
using System.Security.Claims;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Common;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Data;

public class BlogDbContext : IdentityDbContext<User>,IBlogDbContext
{
    private readonly string _currentUserId;
    public BlogDbContext(DbContextOptions<BlogDbContext> options,IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _currentUserId=httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public DbSet<Blog> Blogs { get; set; }
    
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new BlogConfig());
        modelBuilder.ApplyConfiguration(new AuthorConfig());
    }
    
    public override int SaveChanges()
    {
        Update();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Update();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void Update()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                
                if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                
                entry.Property("CreatedBy").CurrentValue = _currentUserId; 
                
            }

            if (entry.State == EntityState.Modified)
            {
                
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
                
                entry.Property("UpdatedBy").CurrentValue = _currentUserId; 
            }
        }
    }



}
