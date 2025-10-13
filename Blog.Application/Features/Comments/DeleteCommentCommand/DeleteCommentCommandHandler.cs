using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;

namespace BlogApi.Application.Features.Comments.DeleteCommentCommand;

public class DeleteCommentCommandHandler:IRequestHandler<DeleteCommentCommand, ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteCommentCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var  comment =await _blogDbContext.Comments.FindAsync(request.CommentId);
       _blogDbContext.Comments.Remove(comment);
       await _blogDbContext.SaveChangesAsync(cancellationToken);
       return new ApiResponse<string>
       {
           Message = "Comment deleted",
       };
    }
}