using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;

public class DeleteBlogWithSpCommandValidator:AbstractValidator<DeleteBlogWithSpCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public DeleteBlogWithSpCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("BlogId is Required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");;

    }
    
}