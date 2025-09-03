using System;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task<User> Update(User user);
    Task Delete(User user);
    Task<User?> GetUserByEmailAsync(string email);
}

public interface IBlogRepository
{
        Task<Blog?> GetByIdAsync(int id);
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog> AddAsync(Blog blog);
        Task<Blog> Update(Blog blog);
        Task Delete(Blog blog);
}
