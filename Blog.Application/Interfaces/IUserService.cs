using System;
using BlogApi.Domain.DTOs;
using BlogApi.Domain.Models;

namespace BlogApi.Application.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterUserAsync(RegisterRequest user);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int userId);

}
