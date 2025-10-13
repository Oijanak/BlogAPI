using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.AuthorFollower.Commands.UnfollowAuthorCommand;

public class UnfollowAuthorCommandHandler:IRequestHandler<UnfollowAuthorCommand,Result<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public UnfollowAuthorCommandHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<string>> Handle(UnfollowAuthorCommand request, CancellationToken cancellationToken)
    {
        var UserId=_currentUserService.UserId;
        var follow = await _blogDbContext.AuthorFollowers
            .FirstOrDefaultAsync(x => x.UserId == UserId && x.AuthorId == request.AuthorId, cancellationToken);

        if (follow == null)
            return Result<string>.Failure("You are not following author", 400);

        _blogDbContext.AuthorFollowers.Remove(follow);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return Result<string>.Success("Unfollowed Author",200);
    }
}