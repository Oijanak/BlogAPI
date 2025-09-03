using System;
using BlogApi.Application.DTOs;


namespace BlogApi.Application.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterUserAsync(CreateUserRequest user);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO?> GetUserByIdAsync(int userId);

    Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest);

    Task<UserDTO> UpdateUserAsync(int userId, UpdateUserRequest updateUser);

    Task DeleteUserAsync(int userId);

    Task<IEnumerable<BlogDTO>> GetBlogsByUserIdAsync(int userId);

}
