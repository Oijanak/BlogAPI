using BlogApi.Domain.Models;
using BlogApi.Domain.Enum;
using System.Text.Json.Serialization;
namespace BlogApi.Application.DTOs;

public class UserDtos
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Roles { get; set; }
    public UserDtos()
    {

    }
    public UserDtos(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
    }
}
public class CreatedByUserDto
{
    public string CreatedById { get; set; }
    public string CreatedByName { get; set; }
    public string CreatedByEmail { get; set; }

    public CreatedByUserDto() { }

    public CreatedByUserDto(User user)
    {
        CreatedById = user.Id;
        CreatedByName = user.Name;
        CreatedByEmail = user.Email;
    }

}

public class UpdatedByUserDto
{
    public string UpdatedById { get; set; }
    public string UpdatedByName { get; set; }
    public string UpdatedByEmail { get; set; }
    public UpdatedByUserDto() { }

    public UpdatedByUserDto(User user) {
        UpdatedById=user.Id;
        UpdatedByName=user.Name;
        UpdatedByEmail = user.Email;
    }

}
public class ApprovedByUserDto
{
    public string ApprovedById { get; set; }
    public string ApprovedByName { get; set; }
    public string ApprovedByEmail { get; set; }

    public ApprovedByUserDto() { }
    public ApprovedByUserDto(User user)
    {
        ApprovedById = user.Id;
        ApprovedByEmail = user.Email;
        ApprovedByName = user.Name;
    }
}
