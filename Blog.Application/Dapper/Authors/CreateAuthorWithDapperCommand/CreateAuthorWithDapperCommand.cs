using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Dapper.Authors.CreateAuthorWithDapperCommand;

public class CreateAuthorWithDapperCommand:IRequest<ApiResponse<AuthorDto>>
{
    public string AuthorEmail { get; set; }
    public string AuthorName { get; set; }
    public int Age { get; set; }
}