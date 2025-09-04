using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQuery:IRequest<UserDTO>
{
    public int UserId { get; }

    public GetUserQuery(int userId)
    {
        UserId = userId;
    }
}