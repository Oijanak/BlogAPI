using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Users.Queries;
using BlogApi.Domain.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResponse<IEnumerable<UserDto>>>
{
    private readonly UserManager<User> _userManager;

    public GetAllUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var userDtos=new List<UserDto>();
        foreach (var user in users)
        {
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Roles = roles
            });
        }

        return new ApiResponse<IEnumerable<UserDto>>
        {
            Message = "Users fetched successfully",
            Data = userDtos
        };
    }
}