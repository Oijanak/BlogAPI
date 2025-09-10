using BlogApi.Application.Features.Authors.Queries.GetBlogsByAuthorIdQuery;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;

public class CreateAuthorCommand:IRequest<AuthorDTO>
{
    public string AuthorEmail { get;}
    public string AuthorName { get; }
    public int Age { get; }

    public CreateAuthorCommand(string authorEmail, string authorName, int age)
    {
        AuthorEmail = authorEmail;
        AuthorName = authorName;
        Age = age;
    }
}

public class AuthorDTO
{
    public Guid AuthorId { get; set; }
    public string AuthorEmail { get; set; }
    public string AuthorName { get; set; }
    public int Age { get; set; }
    public AuthorDTO(){}
    public AuthorDTO(Author author)
    {
        AuthorId = author.AuthorId;
        AuthorEmail = author.AuthorEmail;
        AuthorName = author.AuthorName;
        Age = author.Age;
        
    }

    public AuthorDTO(Guid authorId, string authorEmail, string authorName, int age)
    {
        AuthorId = authorId;
        AuthorEmail = authorEmail;
        AuthorName = authorName;
        Age = age;
    }
}