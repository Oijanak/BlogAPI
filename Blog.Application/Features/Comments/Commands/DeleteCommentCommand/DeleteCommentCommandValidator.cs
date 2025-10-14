using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Comments.DeleteCommentCommand;

public class DeleteCommentCommandValidator:AbstractValidator<DeleteCommentCommand>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteCommentCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.CommentId).NotEmpty().WithMessage("CommentId is required")
            .MustAsync(async (commentId, cancellationToken) =>
                await _blogDbContext.Comments.FindAsync(commentId) != null)
            .WithMessage("Comment not found").WithErrorCode("404");;
    }
}