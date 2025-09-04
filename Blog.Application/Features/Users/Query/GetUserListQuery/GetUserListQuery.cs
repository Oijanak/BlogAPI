using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserListQuery;

public class GetUserListQuery:IRequest<IEnumerable<UserDTO>>
{
    
}