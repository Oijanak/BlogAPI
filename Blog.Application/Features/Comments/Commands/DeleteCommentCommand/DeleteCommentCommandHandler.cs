using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;

namespace BlogApi.Application.Features.Comments.DeleteCommentCommand;

public class DeleteCommentCommandHandler:IRequestHandler<DeleteCommentCommand, Result<string>>
{
    private readonly IBlogDbContext _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCommentCommandHandler(IBlogDbContext blogDbContext,ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<string>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var  comment =await _blogDbContext.Comments.FindAsync(request.CommentId);
       _blogDbContext.Comments.Remove(comment);
       await _blogDbContext.SaveChangesAsync(cancellationToken);
       return Result<string>.Success("Comment deleted");
       
    }
}