
using Microsoft.EntityFrameworkCore; 

using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;

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
        _context.Users.AsEnumerable();
        return Task.FromResult(_context.Users.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(int id)
    {
        _context.Users.Find(id);
        return Task.FromResult(_context.Users.Find(id));
    }

    public void Update(User entity)
    {
        _context.Users.Update(entity);
        _context.SaveChanges();
    }
    
     public async Task<User?> GetUserByEmailAsync(string email)
    {
         User? user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("User not found");
        return user;
    }
}
