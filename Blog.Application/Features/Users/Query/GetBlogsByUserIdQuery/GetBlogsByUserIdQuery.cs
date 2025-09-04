using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetBlogsByUserId;

public class GetBlogsByUserIdQuery:IRequest<IEnumerable<BlogDTO>>
{
    public int UserId { get;}

    public GetBlogsByUserIdQuery(int userId)
    {
        UserId = userId;
    }
    
}