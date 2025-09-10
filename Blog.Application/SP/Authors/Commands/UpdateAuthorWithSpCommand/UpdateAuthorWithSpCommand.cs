using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.SP.Authors.Commands.UpdateAuthorWithSpCommand;

public class UpdateAuthorWithSpCommand:IRequest<AuthorDTO>
{
    public Guid AuthorId { get; }
    public string AuthorEmail { get; }
    public string AuthorName { get; }
    public int Age { get; }

    public UpdateAuthorWithSpCommand(Guid authorId,string authorEmail, string authorName, int age)
    { 
        AuthorId = authorId;
        AuthorEmail = authorEmail;  
        AuthorName = authorName;
        Age = age;
    }
    
}