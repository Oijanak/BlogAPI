using BlogApi.Infrastructure.Data;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryValidator:AbstractValidator<GetBlogQuery>
{
    private readonly BlogDbContext _blogDbContext;
    public GetBlogQueryValidator(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(b => b.BlogId).NotEmpty().WithMessage("BlogId is required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");
    }
}