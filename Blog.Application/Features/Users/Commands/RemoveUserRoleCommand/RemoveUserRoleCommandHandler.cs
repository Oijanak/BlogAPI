using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
namespace BlogApi.Application.Features.Users.Commands.RemoveUserRoleCommand
{
    public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RemoveUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Failure("User not found", 404);
            var roleNames = request.Role.Select(r => r.ToString()).ToList();
            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            if (!result.Succeeded)
            {
                return Result<string>.Failure(result.Errors.First().Description, 400);
            }
            return Result<string>.Success("Roles Removed from user");

        }
    }
}
