using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.AuthorFollower.Queries.GetFollowersQuery;

public class GetFollowersQueryHandler:IRequestHandler<GetFollowersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetFollowersQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<Result<IEnumerable<UserDto>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
    {
        var users=await _blogDbContext.AuthorFollowers
            .Where(x => x.AuthorId == request.AuthorId)
            .Select(x => new  UserDto
            {
                Id = x.User.Id,
                Email = x.User.Email,
                Name = x.User.Name,
            })
            .ToListAsync(cancellationToken);
        return Result<IEnumerable<UserDto>>.Success(users);
    }
}