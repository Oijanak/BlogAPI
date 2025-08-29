
using Microsoft.EntityFrameworkCore; 

using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using BlogApi.Domain.DTOs;

namespace BlogApi.Infrastructure.Repository;
public class UserRepository : IUserRepository

{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext blogDbContext)
    {
        _context = blogDbContext;
    }
    public Task<User> AddAsync(User entity)
    {
        _context.Users.Add(entity);
        _context.SaveChanges();
        return Task.FromResult(entity);
    }

    public void Delete(User entity)
    {
        _context.Users.Remove(entity);
        _context.SaveChanges();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(_context.Users.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(int id)
    {
        
        return Task.FromResult(_context.Users.Find(id));
    }

    public Task<User> Update(User entity)
    {
        _context.Users.Update(entity);
        _context.SaveChanges();
        return Task.FromResult(entity);
    }
    
     public async Task<User?> GetUserByEmailAsync(string email)
    {
        User? user = await _context.Users
       .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }
}
