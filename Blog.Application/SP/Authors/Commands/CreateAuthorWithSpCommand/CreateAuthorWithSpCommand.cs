using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;

public class CreateAuthorWithSpCommand:IRequest<ApiResponse<AuthorDto>>
{
    public string AuthorEmail { get; set; }
    public string AuthorName { get; set; }
    public int Age { get; set; }

    public CreateAuthorWithSpCommand(string authorEmail, string authorName, int age)
    {
        AuthorEmail = authorEmail;
        AuthorName = authorName;
        Age = age;
    }
}