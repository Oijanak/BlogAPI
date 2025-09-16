using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Dapper.Authors.UpdateAuthorWithDapperCommand;

public class UpdateAuthorWithDapperCommand:IRequest<ApiResponse<AuthorDto>>
{
    [FromRoute]
    public Guid AuthorId { get; set; }
    [FromBody]
    public AuthorRequest Author { get; set; }
}