using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Users.Query.GetUserListQuery;

public class GetUserListQueryHandler:IRequestHandler<GetUserListQuery, IEnumerable<UserDTO>>
{
    private readonly BlogDbContext _blogDbContext;

    public GetUserListQueryHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<IEnumerable<UserDTO>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        return await _blogDbContext.Users
            .Select(u => new UserDTO
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email
            })
            .ToListAsync(cancellationToken); 
    }
}