using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;
using BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;
using BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[Route("api/sp/authors")]
[ApiController]
public class AuthorControllerWithSp:ControllerBase
{
    private readonly ISender _sender;

    public AuthorControllerWithSp(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor(AuthorRequest request)
    {
        AuthorDTO author =
            await _sender.Send(new CreateAuthorWithSpCommand(request.AuthorEmail, request.AuthorName, request.Age));
        return Ok(new ApiResponse<AuthorDTO>
        {
            Message = "Author created successfully",
            Data = author
        });
    }

    [HttpPut("{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthor(Guid authorId, AuthorRequest request)
    {
        AuthorDTO author =await _sender.Send(new UpdateAuthorWithSpCommand(authorId, request.AuthorEmail, request.AuthorName,request.Age));
        return Ok(new ApiResponse<AuthorDTO>
        {
            Message = "Author updated successfully",
            Data = author
        });
    }

    [HttpDelete("{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthor(Guid authorId)
    {
        await _sender.Send(new DeleteAuthorWithSpCommand(authorId));
        return Ok(new ApiResponse<string>
        {
            Message = "Author deleted successfully",
        });
    }
}