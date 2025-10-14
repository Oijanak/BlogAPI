using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.BlogFavorite.UnfavoriteBlogCommand;

public class UnfavoriteBlogCommand:IRequest<Result<string>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
}