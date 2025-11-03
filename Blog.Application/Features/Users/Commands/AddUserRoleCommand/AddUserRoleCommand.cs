using BlogApi.Application.DTOs;
using BlogApi.Domain.Enum;
using MediatR;

namespace BlogApi.Application.Features.Users.Commands.AddUserRoles;

public class AddUserRoleCommand:IRequest<Result<string>>
{
    public string UserId { get; set; } = string.Empty;
    public Role[] Role { get; set; } =Array.Empty<Role>();
}