using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.AuthorFollower.Commands.UnfollowAuthorCommand;

public class UnfollowAuthorCommand:IRequest<Result<string>>
{
    public Guid AuthorId { get; set; }
}