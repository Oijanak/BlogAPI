using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQuery:IRequest<ApiResponse<UserDTO>>
{
    [FromRoute]
    public Guid UserId { get; set; }
    
}