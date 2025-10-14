using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.BlogFavorite.FavoriteBlogCommand;

public class FavoriteBlogCommandHandler:IRequestHandler<FavoriteBlogCommand,Result<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public FavoriteBlogCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<string>> Handle(FavoriteBlogCommand request, CancellationToken cancellationToken)
    {
        var UserId = _currentUserService.UserId;
        var exists = await _blogDbContext.BlogFavorites
            .AnyAsync(f => f.UserId == UserId && f.BlogId == request.BlogId, cancellationToken);

        if (exists)
            return Result<string>.Failure("Already Favorited");
        var favorite = new Domain.Models.BlogFavorite
        {
            UserId = UserId,
            BlogId = request.BlogId,
            FavoritedOn = DateTime.UtcNow
        };

        _blogDbContext.BlogFavorites.Add(favorite);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return Result<string>.Success("Blog is favorited");
    }
}