using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;

public class GetAuthorsWithAgeQuery:IRequest<IEnumerable<AuthorDTO>>
{
  public int Age1 { get; }
  public int Age2 { get; }

  public GetAuthorsWithAgeQuery(int ag1, int ag2)
  {
    Age1 = ag1; 
    Age2 = ag2;
  }
}