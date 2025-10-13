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
    private readonly ICurrentUserService _currentUserService;
    public BlogDbContext(DbContextOptions<BlogDbContext> options,ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }
    public DbSet<Blog> Blogs { get; set; }
    
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<BlogDocument> BlogDocument { get; set; }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<AuthorFollower> AuthorFollowers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new BlogConfig());
        modelBuilder.ApplyConfiguration(new AuthorConfig());
        modelBuilder.ApplyConfiguration(new CategoryConfig());
        modelBuilder.ApplyConfiguration(new BlogDocumentConfig());
        modelBuilder.ApplyConfiguration(new CommentConfig());
        modelBuilder.ApplyConfiguration(new AuthorFollowerConfig());
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
        var currentUserId=_currentUserService.UserId;
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                
                if (entry.Properties.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }
                
                entry.Property("CreatedBy").CurrentValue = currentUserId; 
                
            }

            if (entry.State == EntityState.Modified)
            {
                
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdatedAt"))
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
                
                entry.Property("UpdatedBy").CurrentValue = currentUserId ?? entry.Property("UpdatedBy").CurrentValue;
                
            }
        }
    }



}
