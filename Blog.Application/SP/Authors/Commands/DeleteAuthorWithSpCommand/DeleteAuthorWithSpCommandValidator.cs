using BlogApi.Infrastructure.Data;
using FluentValidation;

namespace BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;

public class DeleteAuthorWithSpCommandValidator:AbstractValidator<DeleteAuthorWithSpCommand>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteAuthorWithSpCommandValidator(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
    }

}