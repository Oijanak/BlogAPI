using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryValidator:AbstractValidator<GetBlogQuery>
{
    private readonly IBlogDbContext _blogDbContext;
    public GetBlogQueryValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(b => b.BlogId).NotEmpty().WithMessage("BlogId is required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");
    }
}