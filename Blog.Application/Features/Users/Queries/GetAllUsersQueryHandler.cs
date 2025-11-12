using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Users.Queries;
using BlogApi.Domain.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResponse<IEnumerable<UserDtos>>>
{
    private readonly UserManager<User> _userManager;

    public GetAllUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApiResponse<IEnumerable<UserDtos>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var userDtos=new List<UserDtos>();
        foreach (var user in users)
        {
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            userDtos.Add(new UserDtos
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Roles = roles
            });
        }

        return new ApiResponse<IEnumerable<UserDtos>>
        {
            Message = "Users fetched successfully",
            Data = userDtos
        };
    }
}