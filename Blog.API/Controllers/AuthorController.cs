using Blog.API.Filters;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Authors.Commands.DeleteAuthorCommand;
using BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;
using BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;
using BlogApi.Application.Features.Authors.Queries.GetAuthorListQuery;
using BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;
using BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;
using BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;
using BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;
using BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;
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
        return Created("",await _sender.Send(new CreateAuthorCommand(author.AuthorEmail,author.AuthorName,author.Age)));
    }
    [HttpPut("{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthor(Guid authorId,AuthorRequest author)
    {
        return Ok(await _sender.Send(new UpdateAuthorCommand(authorId,author.AuthorEmail,author.AuthorName,author.Age)));
    }

    [HttpDelete("{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthor(Guid authorId)
    {
        return Ok(await _sender.Send(new DeleteAuthorCommand(authorId)));
    }

    [HttpGet("{authorId:guid}")]
    public async Task<IActionResult> GetAuthorById(Guid authorId)
    {
        return Ok(await _sender.Send(new GetAuthorByIdQuery(authorId)));
    }
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        return Ok(await _sender.Send(new GetAuthorListQuery()));
    }

    [HttpGet("{authorId:guid}/blogs")]
    [AgeRequirement]
    public async Task<IActionResult> GetBlogsByAuthorId(Guid authorId)
    {
        return Ok(await _sender.Send(new GetBlogsByAuthorIdQuery(authorId)));
    }

    [HttpGet("age")]
    public async Task<IActionResult> GetAuthorsByAgeBetween([FromQuery]int age1,[FromQuery]int age2)
    {
       return Ok(await _sender.Send(new GetAuthorsWithAgeQuery(age1, age2)));
    }
    
    [HttpPost("sp")]
    public async Task<IActionResult> CreateAuthorWithSp(AuthorRequest request)
    {
         return  Created("",await _sender.Send(new CreateAuthorWithSpCommand(request.AuthorEmail, request.AuthorName, request.Age)));
    }

    [HttpPut("sp/{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthorWithSp(Guid authorId, AuthorRequest request)
    {
        return Ok(await _sender.Send(new UpdateAuthorWithSpCommand(authorId, request.AuthorEmail, request.AuthorName,request.Age)));
    }

    [HttpDelete("sp/{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthorWithSp(Guid authorId)
    {
       return Ok(await _sender.Send(new DeleteAuthorWithSpCommand(authorId)));
    }
}



