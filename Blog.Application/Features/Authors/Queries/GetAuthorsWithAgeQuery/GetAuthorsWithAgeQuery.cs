using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;

public class GetAuthorsWithAgeQuery:IRequest<ApiResponse<IEnumerable<AuthorDto>>>
{
  [FromQuery]
  public int Age1 { get; }
  [FromQuery]
  public int Age2 { get; }

  public GetAuthorsWithAgeQuery(int ag1, int ag2)
  {
    Age1 = ag1; 
    Age2 = ag2;
  }
}