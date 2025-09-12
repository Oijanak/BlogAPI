using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQuery:IRequest<ApiResponse<AuthorDto>>
{
    [FromRoute]
    public Guid AuthorId { get; set; }

    public GetAuthorByIdQuery(Guid authorId)
    {
        AuthorId = authorId;
    }
    
}