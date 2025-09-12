using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;


public class UpdateAuthorCommand:IRequest<ApiResponse<AuthorDto>>
{
    [FromRoute]
    public Guid AuthorId { get; set; }
    [FromBody]
    public AuthorRequest Author { get; init; }
    
}