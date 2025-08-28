using System;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task<User> CreateUserAsync(User user)
    {
        return _userRepository.AddAsync(user);
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllAsync();
    }


    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return  await _userRepository.GetByIdAsync(userId);
    }
}
