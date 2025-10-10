using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogDocumentQuery;

public class GetBlogDocumentQueryValidator:AbstractValidator<GetBlogDocumentQuery>
{
    private readonly IBlogDbContext _blogDbContext;
    public GetBlogDocumentQueryValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
         RuleFor(bc => bc.BlogDocumentId).NotEmpty().WithMessage("BlogDocumentId is required")
                    .MustAsync(async (blogDocumentId, cancellationToken) =>
                        await _blogDbContext.BlogDocument.FindAsync(blogDocumentId) != null)
                    .WithMessage("BlogDocument not found").WithErrorCode("404");
    }
    

}