using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;


public class UpdateAuthorCommand:IRequest<AuthorDTO>
{
    public Guid AuthorId { get; }
    public string AuthorEmail { get; }
    public string AuthorName { get; }
    public int Age { get; }

    public UpdateAuthorCommand(Guid authorId,string authorEmail, string authorName, int age)
    { AuthorId = authorId;
      AuthorEmail = authorEmail;  
      AuthorName = authorName;
      Age = age;
    }
    
    
}