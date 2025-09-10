using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.SP.Authors.Commands.CreateAuthorWithSpCommand;

public class CreateAuthorWithSpCommand:IRequest<AuthorDTO>
{
    public string AuthorEmail { get;}
    public string AuthorName { get; }
    public int Age { get; }

    public CreateAuthorWithSpCommand(string authorEmail, string authorName, int age)
    {
        AuthorEmail = authorEmail;
        AuthorName = authorName;
        Age = age;
    }
}