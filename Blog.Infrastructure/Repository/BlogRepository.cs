using System;
using BlogApi.Domain.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;


namespace BlogApi.Infrastructure.Repository;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;


    public BlogRepository(BlogDbContext blogDbContext)
    {
        _context = blogDbContext;
        
    }
    public Task<Blog> AddAsync(Blog entity)
    {
        _context.Blogs.Add(entity);
        _context.SaveChanges();
        return Task.FromResult(entity);

    }

    public  Task Delete(Blog entity)
    {
        _context.Blogs.Remove(entity);
        return _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Blog>> GetAllAsync()
    {
        return Task.FromResult(_context.Blogs.AsEnumerable());
    }

    public Task<Blog?> GetByIdAsync(int id)
    {
        return Task.FromResult(_context.Blogs.Find(id));
    }

    public Task<Blog> Update(Blog entity)
    {
        _context.Blogs.Update(entity);
        _context.SaveChanges();
        return Task.FromResult(entity);
        
    }

}
