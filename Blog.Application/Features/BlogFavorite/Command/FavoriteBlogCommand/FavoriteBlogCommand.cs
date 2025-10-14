using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.BlogFavorite.FavoriteBlogCommand;

public class FavoriteBlogCommand:IRequest<Result<string>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
}