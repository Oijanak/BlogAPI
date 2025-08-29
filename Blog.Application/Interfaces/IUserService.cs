using System;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterUserAsync(UserRequest user);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int userId);

    Task<ApiResponse<string>> LoginUserAsync(LoginRequest loginRequest);

    Task<UserDTO> UpdateUserAsync(int userId, UpdateUserRequest updateUser);

    Task DeleteUserAsync(int userId);

}
