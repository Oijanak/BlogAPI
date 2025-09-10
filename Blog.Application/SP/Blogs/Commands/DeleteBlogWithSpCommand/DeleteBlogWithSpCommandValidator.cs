using BlogApi.Infrastructure.Data;
using FluentValidation;

namespace BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;

public class DeleteBlogWithSpCommandValidator:AbstractValidator<DeleteBlogWithSpCommand>
{
    private readonly BlogDbContext _blogDbContext;
    public DeleteBlogWithSpCommandValidator(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("BlogId is Required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");;

    }
    
}