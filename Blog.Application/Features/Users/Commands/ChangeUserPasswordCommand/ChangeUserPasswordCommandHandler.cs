using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BlogApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using BlogApi.Application.Interfaces;

namespace BlogApi.Application.Features.Users.Commands.ChangeUserPasswordCommand
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public ChangeUserPasswordCommandHandler(UserManager<User> userManager,ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService=currentUserService;
        }
        public async Task<Result<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user == null) 
                return Result<string>.Failure("User not found",404);
          
             var result = await _userManager.ChangePasswordAsync(user,request.OldPassword, request.NewPassword);
            if(!result.Succeeded) 
                return Result<string>.Failure(result.Errors.First().Description, 400);
            return Result<string>.Success("Password Changed Successfully");
        }
    }
}
