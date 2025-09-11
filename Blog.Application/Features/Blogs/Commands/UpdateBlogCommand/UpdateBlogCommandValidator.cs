using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandValidator:AbstractValidator<UpdateBlogCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public UpdateBlogCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog Id is Required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");;
        RuleFor(x => x.BlogTitle).MaximumLength(200).WithMessage("Blog Title should be of length 200 characters");
        RuleFor(x=>x.AuthorId).NotEmpty().WithMessage("Author Id is Required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.BlogTitle) || !string.IsNullOrWhiteSpace(x.BlogContent))
            .WithMessage("At least BlogTitle or BlogContent must have a value.");
    }
}