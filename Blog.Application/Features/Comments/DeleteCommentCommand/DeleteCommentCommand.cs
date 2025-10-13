using BlogApi.Application.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Comments.DeleteCommentCommand;

public class DeleteCommentCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid CommentId { get; set; }
    
}