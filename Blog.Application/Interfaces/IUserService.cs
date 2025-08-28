using System;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId); 

}
