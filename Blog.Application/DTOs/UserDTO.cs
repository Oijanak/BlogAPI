using System.ComponentModel.DataAnnotations;

namespace BlogApi.Application.DTOs;

public class UserDTO
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<BlogDTO> Blogs { get; set; } = [];
}

public class UpdateUserRequest
{
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
}