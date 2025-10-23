using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Comments.UpdateCommentCommand;

public class UpdateCommentCommandValidator:AbstractValidator<UpdateCommentCommand>
{
    private readonly IBlogDbContext _blogDbContext;

    public UpdateCommentCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.CommentId).NotEmpty().WithMessage("CommentId is required")
            .MustAsync(async (commentId, cancellationToken) =>
                await _blogDbContext.Comments.FindAsync(commentId) != null)
            .WithMessage("Comment not found").WithErrorCode("404");;
        RuleFor(x=>x.UpdateCommentRequest.Content).NotEmpty().WithMessage("Content is required").
            MaximumLength(500).WithMessage("Content should be of length 500 characters");
    }
}