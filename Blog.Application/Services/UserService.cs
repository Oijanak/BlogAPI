using System;
using System.Net;
using BlogApi.Domain.Interfaces;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Exceptions;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
   
    public async Task<UserDTO> RegisterUserAsync(UserRequest user)
    {

        User? existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingUser is not null)
        {
            throw new ApiException("Email Already Registered", HttpStatusCode.BadRequest);
        }
        User newUser = new()
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
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
            Email = u.Email,
            Blogs = u.Blogs.Select(b => new BlogDTO
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogContent = b.BlogContent,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
            }).ToList()
        });
    }

    public async Task<UserDTO?> GetUserByIdAsync(int userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId) ?? throw new ApiException("User not found with id " + userId, HttpStatusCode.NotFound);
        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Blogs = user.Blogs.Select(b => new BlogDTO
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogContent = b.BlogContent,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
            }).ToList()
        };
    }
    public async Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest)
    {
        User user = await _userRepository.GetUserByEmailAsync(loginRequest.Email) ?? throw new ApiException("Invalid Email or Password", HttpStatusCode.Unauthorized);

        bool isPasswordValid = user.VerifyPassword(loginRequest.Password);
        if (!isPasswordValid)
        {
            throw new ApiException("Invalid Email or Password", HttpStatusCode.Unauthorized);
        }
        string token = _tokenService.GenerateToken(user.UserId, user.Email);
        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1)    
        };

    }

    public async Task<UserDTO> UpdateUserAsync(int userId, UpdateUserRequest updateUser)
    {
        User user = await _userRepository.GetByIdAsync(userId) ?? throw new ApiException("User not found with id " + userId, HttpStatusCode.NotFound);
        user.Name = updateUser.Name ?? user.Name;
        user.Email = updateUser.Email ?? user.Email;
        if (updateUser.Password is not null)
            user.Password = updateUser.Password;
        User updatedUser = await _userRepository.Update(user);
        return new UserDTO
        {
            UserId = updatedUser.UserId,
            Name = updatedUser.Name,
            Email = updatedUser.Email
        };

    }
    public async Task DeleteUserAsync(int userId)
    {
        User user = await _userRepository.GetByIdAsync(userId) ?? throw new ApiException("User not found with id " + userId, HttpStatusCode.NotFound);
        await _userRepository.Delete(user);
    }

    public async Task<IEnumerable<BlogDTO>> GetBlogsByUserIdAsync(int userId)
    {
        User user = await _userRepository.GetByIdAsync(userId) ?? throw new ApiException("User not found with id " + userId, HttpStatusCode.NotFound);
        return user.Blogs.Select(b => new BlogDTO
        {
            BlogId = b.BlogId,
            BlogTitle = b.BlogTitle,
            BlogContent = b.BlogContent,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt,
        });
    }
}
