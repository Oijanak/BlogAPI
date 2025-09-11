using BlogApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlogApi.Application.Interfaces;

public interface IBlogDbContext
{
    DbSet<Author> Authors { get; }
    DbSet<User> Users { get; }
    DbSet<Blog> Blogs { get;  }
    
    DatabaseFacade Database { get; } 
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}