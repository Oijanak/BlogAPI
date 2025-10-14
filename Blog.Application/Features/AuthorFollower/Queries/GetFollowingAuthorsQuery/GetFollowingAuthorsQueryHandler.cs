using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.AuthorFollower.Queries.GetFollowingAuthorsQuery;

public class GetFollowingAuthorsQueryHandler:IRequestHandler<GetFollowingAuthorsQuery, Result<IEnumerable<AuthorDto>>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetFollowingAuthorsQueryHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext=blogDbContext;
        _currentUserService=currentUserService;
    }
    public async Task<Result<IEnumerable<AuthorDto>>> Handle(GetFollowingAuthorsQuery request, CancellationToken cancellationToken)
    {
        var UserId = _currentUserService.UserId;
        var authors= await _blogDbContext.AuthorFollowers
            .Where(x => x.UserId == UserId)
            .Select(x => new AuthorDto
            {
                AuthorId = x.Author.AuthorId,
                AuthorName = x.Author.AuthorName,
                AuthorEmail = x.Author.AuthorEmail,
                Age = x.Author.Age,
                CreatedBy = x.Author.CreatedBy,
            })
            .ToListAsync(cancellationToken);
        return Result<IEnumerable<AuthorDto>>.Success(authors);
    }
}