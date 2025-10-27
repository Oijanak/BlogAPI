using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Comments.Commands.ReactToCommentCommand;

public class ReactCommentCommandValidator : AbstractValidator<ReactToCommentCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public ReactCommentCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.CommentId).NotEmpty().WithMessage("CommentId is required")
            .MustAsync(async (commentId, cancellationToken) =>
                await _blogDbContext.Comments.FindAsync(commentId) != null)
            .WithMessage("Comment not found").WithErrorCode("404");;
    }

}