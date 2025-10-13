using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Comments.CreateCommentCommand;

public class CreateCommentCommandValidator:AbstractValidator<CreateCommentCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public CreateCommentCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.BlogId).NotEmpty().WithMessage("BlogId is required.")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");
        RuleFor(x=>x.Content).NotEmpty().WithMessage("Content is required").
            MaximumLength(500).WithMessage("Content should be of length 500 characters");
    }

}