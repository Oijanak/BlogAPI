using System;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Models;

namespace BlogApi.Domain.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterUserAsync(UserRequest user);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int userId);

    Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest);

    Task<UserDTO> UpdateUserAsync(int userId, UpdateUserRequest updateUser);

    Task DeleteUserAsync(int userId);

    Task<IEnumerable<BlogDTO>> GetBlogsByUserIdAsync(int userId);

}
