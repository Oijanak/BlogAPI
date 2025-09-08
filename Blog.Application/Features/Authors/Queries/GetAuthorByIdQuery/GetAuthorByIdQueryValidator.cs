using BlogApi.Infrastructure.Data;
using FluentValidation;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQueryValidator:AbstractValidator<GetAuthorByIdQuery>
{
    private readonly BlogDbContext _blogDbContext;

    public GetAuthorByIdQueryValidator(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
    } 
}