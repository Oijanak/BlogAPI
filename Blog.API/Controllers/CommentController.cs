using BlogApi.Application.Features.Comments.CreateCommentCommand;
using BlogApi.Application.Features.Comments.DeleteCommentCommand;
using BlogApi.Application.Features.Comments.UpdateCommentCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[Route("api/comments")]
[ApiController]
public class CommentController:ControllerBase
{
    private readonly ISender _sender;

    public CommentController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateComment(CreateCommentCommand createCommentCommand)
    {
        return StatusCode(StatusCodes.Status201Created,await _sender.Send(createCommentCommand));
    }

    [HttpPut("{CommentId}")]
    [Authorize]
    public async Task<IActionResult> UpdateComment(UpdateCommentCommand updateCommentCommand)
    {
        return Ok(await _sender.Send(updateCommentCommand));
    }

    [HttpDelete("{CommentId}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(DeleteCommentCommand deleteCommentCommand)
    {
        return Ok(await _sender.Send(deleteCommentCommand));
    }
}