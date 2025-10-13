using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.AuthorFollower.Queries.GetFollowersQuery;

public class GetFollowersQuery:IRequest<Result<IEnumerable<UserDto>>>
{
   [FromRoute]
   public Guid AuthorId { get; set; } 
}