using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.AuthorFollower.FollowAuthorCommand;

public class FollowAuthorCommand:IRequest<Result<string>>
{
    [FromRoute]
    public Guid AuthorId { get; set; }
   
}