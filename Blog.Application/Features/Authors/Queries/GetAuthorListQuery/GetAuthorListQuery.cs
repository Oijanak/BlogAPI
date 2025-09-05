using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorListQuery;

public class GetAuthorListQuery:IRequest<IEnumerable<AuthorDTO>>
{
    
}