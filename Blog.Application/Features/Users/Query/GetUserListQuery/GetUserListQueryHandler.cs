using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Users.Query.GetUserListQuery;

public class GetUserListQueryHandler:IRequestHandler<GetUserListQuery, ApiResponse<IEnumerable<UserDTO>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetUserListQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<IEnumerable<UserDTO>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users=await _blogDbContext.Users
            .Select(u => new UserDTO(u))
            .ToListAsync(cancellationToken);
        return new ApiResponse<IEnumerable<UserDTO>>
        {
            Data = users,
            Message = "Users fetched successfully"
        };
    }
}