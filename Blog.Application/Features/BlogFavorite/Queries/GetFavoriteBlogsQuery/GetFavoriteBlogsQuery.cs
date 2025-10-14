using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.BlogFavorite.Queries.GetFavoriteBlogsQuery;

public class GetFavoriteBlogsQuery:IRequest<Result<IEnumerable<BlogDTO>>>
{
    
}