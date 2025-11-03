using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Application.Features.Users.Commands.AddUserRoles;

public class AddUserRoleCommandHandler
{
    public class AddUserRolesCommandHandler : IRequestHandler<AddUserRoleCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddUserRolesCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<string>> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Failure("User not found",404);
            
            foreach (var role in request.Role)
            {
                var roleName = role.ToString();

                if (!await _roleManager.RoleExistsAsync(roleName))
                    throw new Exception($"Role '{roleName}' does not exist");

                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                       return Result<string>.Failure(result.Errors.First().Description, 400);
                }
            }

            return Result<string>.Success("Roles added to the user");
        }
    }
}