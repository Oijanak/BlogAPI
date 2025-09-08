using Blog.API.Filters;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;
using BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;
using BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;
using BlogApi.Application.Features.Authors.Queries.GetAuthorListQuery;
using BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;
using BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.API.Controllers;
[Route("api/authors")]
[ApiController]
public class AuthorController:ControllerBase
{
    private readonly ISender _sender;
    public AuthorController(ISender sender)
        => _sender = sender;
    [HttpPost]
    public async Task<IActionResult> CreateAuthor(AuthorRequest author)
    {
        AuthorDTO createdAuthor = await _sender.Send(new CreateAuthorCommand(author.AuthorEmail,author.AuthorName,author.Age));
        return Ok(new ApiResponse<AuthorDTO>
        {
            Message = "Author created successfully",
            Data = createdAuthor
        });
    }
    [HttpPut("{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthor(Guid authorId,AuthorRequest author)
    {
        AuthorDTO createdAuthor = await _sender.Send(new UpdateAuthorCommand(authorId,author.AuthorEmail,author.AuthorName,author.Age));
        return Ok(new ApiResponse<AuthorDTO>
        {
            Message = "Author updated successfully",
            Data = createdAuthor
        });
    }

    [HttpDelete("{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthor(Guid authorId)
    {
        await _sender.Send(new DeleteAuthorCommand(authorId));
        return Ok(new ApiResponse<string>
        {
            Message = "Author deleted successfully",
        });
    }

    [HttpGet("{authorId:guid}")]
    public async Task<IActionResult> GetAuthorById(Guid authorId)
    {
        AuthorDTO author = await _sender.Send(new GetAuthorByIdQuery(authorId));
        return Ok(new ApiResponse<AuthorDTO>
        {
            Message = "Author Fetched successfully",
            Data = author
        });
    }
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        IEnumerable<AuthorDTO> authors = await _sender.Send(new GetAuthorListQuery());
        return Ok(new ApiResponse<IEnumerable<AuthorDTO>>
        {
            Message = "Author Fetched successfully",
            Data = authors
        });
    }

    [HttpGet("{authorId:guid}/blogs")]
    [AgeRequirement]
    public async Task<IActionResult> GetBlogsByAuthorId(Guid authorId)
    {
        IEnumerable<BlogDTO> blogDtos = await _sender.Send(new GetBlogsByAuthorIdQuery(authorId));
        return Ok(new ApiResponse<IEnumerable<BlogDTO>>
        {
            Message = "Author Blogs fetched successfully",
            Data = blogDtos,
        });
    }

    [HttpGet("age")]
    public async Task<IActionResult> GetAuthorsByAgeBetween([FromQuery]int age1,[FromQuery]int age2)
    {
        IEnumerable<AuthorDTO> authors = await _sender.Send(new GetAuthorsWithAgeQuery(age1, age2));
        return Ok(new ApiResponse<IEnumerable<AuthorDTO>>
        {
            Message = $"Author Between age {age1} and {age2} fetched successfully",
            Data = authors,
        });
    }
}



