using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Comments.Commands.ReactToCommentCommand;

public class ReactToCommentCommandHandler: IRequestHandler<ReactToCommentCommand, Result<ReactToCommentDto>>
{
    private readonly IBlogDbContext  _blogDbContext;
    private readonly ICurrentUserService _currentUserService;

    public ReactToCommentCommandHandler(IBlogDbContext blogDbContext, ICurrentUserService currentUserService)
    {
        _blogDbContext = blogDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<Result<ReactToCommentDto>> Handle(ReactToCommentCommand request, CancellationToken cancellationToken)
    {
        var reaction = await _blogDbContext.CommentReactions
            .FirstOrDefaultAsync(cr => cr.CommentId == request.CommentId && cr.UserId == _currentUserService.UserId, cancellationToken);
        if (reaction != null)
        {
            if (reaction.IsLike == request.IsLike)
            {
                _blogDbContext.CommentReactions.Remove(reaction);
                await _blogDbContext.SaveChangesAsync(cancellationToken);
                var response = new ReactToCommentDto
                {
                    CommentId = request.CommentId,
                    IsLike = request.IsLike
                };
                return Result<ReactToCommentDto>.Success(response);
            }
            else
            {
                
                reaction.IsLike = request.IsLike;
            }
        }
        else
        {
           
            _blogDbContext.CommentReactions.Add(new CommentReaction()
            {
                CommentId = request.CommentId,
                UserId = _currentUserService.UserId,
                IsLike = request.IsLike
            });
        }

        await _blogDbContext.SaveChangesAsync(cancellationToken);
        var responseDto = new ReactToCommentDto
        {
            CommentId = request.CommentId,
            IsLike = request.IsLike
        };
        return Result<ReactToCommentDto>.Success(responseDto,201);
        
    }
}