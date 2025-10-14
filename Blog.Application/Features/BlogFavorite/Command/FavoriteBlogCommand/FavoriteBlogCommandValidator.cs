using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.BlogFavorite.FavoriteBlogCommand;

public class FavoriteBlogCommandValidator: AbstractValidator<FavoriteBlogCommand>
{
    private readonly IBlogDbContext _blogDbContext;

    public FavoriteBlogCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog Id is Required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404"); 
    }
    
}