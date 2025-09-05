using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQuery:IRequest<AuthorDTO>
{
    public int AuthorId { get; set; }

    public GetAuthorByIdQuery(int authorId)
    {
        AuthorId = authorId;
    }
    
}