using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.AuthorFollower.Queries.GetFollowersQuery;

public class GetFollowersQueryValidator:AbstractValidator<GetFollowersQuery>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetFollowersQueryValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.AuthorId).NotEmpty()
            .WithMessage("AuthorId is required.")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
        
    }
    
    
}