using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;

public class GetBlogsByAuthorQueryValidator:AbstractValidator<GetBlogsByAuthorIdQuery>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetBlogsByAuthorQueryValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
    } 
    
}