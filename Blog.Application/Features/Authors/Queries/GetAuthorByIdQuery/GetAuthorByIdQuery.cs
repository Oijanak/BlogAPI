using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorByIdCommand;

public class GetAuthorByIdQuery:IRequest<AuthorDTO>
{
    public Guid AuthorId { get; set; }

    public GetAuthorByIdQuery(Guid authorId)
    {
        AuthorId = authorId;
    }
    
}