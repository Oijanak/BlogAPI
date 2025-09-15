using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;

public class CreateAuthorCommand:IRequest<ApiResponse<AuthorDto>>
{
    public string AuthorEmail { get; set; }
    public string AuthorName { get; set; }
    public int Age { get; set; }
    public CreateAuthorCommand(){}
    public CreateAuthorCommand(string authorEmail, string authorName, int age)
    {
        AuthorEmail = authorEmail;
        AuthorName = authorName;
        Age = age;
    }
}

