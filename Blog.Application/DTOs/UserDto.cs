using BlogApi.Domain.Models;

namespace BlogApi.Application.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserDto()
    {

    }
    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
    }
}