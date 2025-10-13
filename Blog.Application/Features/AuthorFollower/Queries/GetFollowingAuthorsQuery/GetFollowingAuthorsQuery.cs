using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.AuthorFollower.Queries.GetFollowingAuthorsQuery;

public class GetFollowingAuthorsQuery:IRequest<Result<IEnumerable<AuthorDto>>>
{
    
}