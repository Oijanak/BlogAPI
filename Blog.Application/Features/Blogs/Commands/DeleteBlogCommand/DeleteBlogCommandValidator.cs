using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommandValidator: AbstractValidator<DeleteBlogCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public DeleteBlogCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("BlogId is Required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");;

    }
    
}