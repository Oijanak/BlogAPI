using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.AuthorFollower.FollowAuthorCommand;

public class FollowAuthorCommandHandler:IRequestHandler<FollowAuthorCommand,Result<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public FollowAuthorCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<string>> Handle(FollowAuthorCommand request, CancellationToken cancellationToken)
    {
        var UserId=_currentUserService.UserId;
        var exists = await _blogDbContext.AuthorFollowers
            .AnyAsync(x => x.UserId == UserId && x.AuthorId == request.AuthorId, cancellationToken);

        if (exists)
            return Result<string>.Failure("Already Following Author", 400);

        _blogDbContext.AuthorFollowers.Add(new Domain.Models.AuthorFollower
        {
            UserId = UserId,
            AuthorId = request.AuthorId,
            FollowedOn = DateTime.UtcNow
        });

        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return Result<string>.Success($"You are following Author with author Id {request.AuthorId}",200);
    }
}