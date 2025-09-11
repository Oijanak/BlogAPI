using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQuery:IRequest<ApiResponse<UserDTO>>
{
    public Guid UserId { get; }

    public GetUserQuery(Guid userId)
    {
        UserId = userId;
    }
}