using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.BlogFavorite.UnfavoriteBlogCommand;

public class UnfavoriteBlogCommandHandler:IRequestHandler<UnfavoriteBlogCommand, Result<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private ICurrentUserService _currentUserService;

    public UnfavoriteBlogCommandHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<string>> Handle(UnfavoriteBlogCommand request, CancellationToken cancellationToken)
    {
       var UserId = _currentUserService.UserId;
       var favorite = await _blogDbContext.BlogFavorites
           .FirstOrDefaultAsync(f => f.UserId == UserId && f.BlogId == request.BlogId, cancellationToken);

       if (favorite == null)
           return Result<string>.Failure("No favorite found",404);

       _blogDbContext.BlogFavorites.Remove(favorite);
       await _blogDbContext.SaveChangesAsync(cancellationToken);
       return Result<string>.Success("Unfavorited Blog");
    }
}