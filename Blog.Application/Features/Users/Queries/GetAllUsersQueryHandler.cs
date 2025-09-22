using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Users.Queries;
using BlogApi.Domain.Models;
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
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Name=u.Name
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<IEnumerable<UserDto>>
        {
            Message = "Users fetched successfully",
            Data = users
        };
    }
}