using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using BlogApi.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApi.Application.Interfaces;

namespace BlogApi.Application.Features.Users.Commands.AddUserAsMakerCommand
{
    public class AddUserMakerCommandHandler : IRequestHandler<AddUserAsMakerCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBlogDbContext _blogDbContext;

        public AddUserMakerCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IBlogDbContext blogDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _blogDbContext = blogDbContext;
        }
        public async Task<Result<string>> Handle(AddUserAsMakerCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) 
                return Result<string>.Failure("User not found", 404);

            var roleExist = await _roleManager.RoleExistsAsync(Role.Maker.ToString());
            if (!roleExist)
                return Result<string>.Failure("Role doesn't exist", 404);
            var isMaker=await _userManager.IsInRoleAsync(user,Role.Maker.ToString());
            if (isMaker)
                return Result<string>.Success("User added to role Maker");
            var author = new Author()
            {
                AuthorName = user.Name,
                AuthorEmail=user.Email
            };
            await _blogDbContext.Authors.AddAsync(author);
            var result= await _userManager.AddToRoleAsync(user,Role.Maker.ToString());
            if (!result.Succeeded)
                return Result<string>.Success("Failed to add user to role Maker");
            await _blogDbContext.SaveChangesAsync();
            return Result<string>.Success("User added to role Maker");

        }
    }
}
