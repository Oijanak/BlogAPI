using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllBlogCommentsQuery;

public class GetAllBlogsCommentsQueryValidator:AbstractValidator<GetAllBlogCommentsQuery>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetAllBlogsCommentsQueryValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext=blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("BlogId is required.")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");;
    }
    
}