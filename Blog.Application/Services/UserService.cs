using System;
using System.Net;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Exceptions;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserDTO> RegisterUserAsync(RegisterRequest user)
    {

        User? existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingUser is not null)
        {
            throw new ApiException("Email Already Registered",HttpStatusCode.BadRequest);
        }
        User newUser = new()
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };
        User createdUser = await _userRepository.AddAsync(newUser);
        return new UserDTO()
        {
            UserId = createdUser.UserId,
            Name = createdUser.Name,
            Email = createdUser.Email,
        };
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserDTO
        {
            UserId = u.UserId,
            Name = u.Name,
            Email = u.Email
        });
    }

    public async Task<UserDTO?> GetUserByIdAsync(int userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId) ?? throw new ApiException("User not found with id "+userId,HttpStatusCode.NotFound );
        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email
        };
    }
    
}
