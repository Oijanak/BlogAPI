using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;

public class UpdateAuthorWithSpCommand:IRequest<ApiResponse<AuthorDto>>
{
    [FromRoute]
    public Guid AuthorId { get; }
    [FromBody]
    public AuthorRequest Author { get; set; }
    
}