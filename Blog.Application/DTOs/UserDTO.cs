using System.ComponentModel.DataAnnotations;
using BlogApi.Domain.Models;

namespace BlogApi.Application.DTOs;

public class UserDTO
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public UserDTO(User user)
    {
        UserId = user.UserId;
        Name = user.Name;
        Email = user.Email;
    }
    
}

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
