using BlogApi.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Comments.CreateCommentCommand;

public class CreateCommentCommandValidator:AbstractValidator<CreateCommentCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public CreateCommentCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.BlogId).NotEmpty().WithMessage("BlogId is required.")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");
        RuleFor(x=>x.Content).NotEmpty().WithMessage("Content is required").
            MaximumLength(500).WithMessage("Content should be of length 500 characters");
        
        RuleFor(x => x.ParentCommentId)
            .MustAsync(async (parentId, cancellationToken) =>
            {
                if (parentId == null)
                    return true;

                
                return await _blogDbContext.Comments
                    .AnyAsync(c => c.CommentId == parentId, cancellationToken);
            })
            .WithMessage("Parent comment not found.")
            .WithErrorCode("404");
        
        RuleFor(c => c)
            .MustAsync(BeValidParentComment)
            .WithMessage("Parent comment must belong to the same blog");
    }

    private async Task<bool> BeValidParentComment(CreateCommentCommand command, CancellationToken token)
    {
        if (!command.ParentCommentId.HasValue)
            return true; 

        var parent = await _blogDbContext.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CommentId == command.ParentCommentId.Value, token);

        if (parent == null)
            return false;

        return parent.BlogId == command.BlogId;
    }

}