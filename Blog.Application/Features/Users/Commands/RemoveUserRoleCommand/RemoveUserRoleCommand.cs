using BlogApi.Application.DTOs;
using BlogApi.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Users.Commands.RemoveUserRoleCommand
{
    public class RemoveUserRoleCommand:IRequest<Result<string>>
    {
        public string UserId { get; set; } = string.Empty;
        public List<Role> Role { get; set; } = new List<Role>();
    }
}
