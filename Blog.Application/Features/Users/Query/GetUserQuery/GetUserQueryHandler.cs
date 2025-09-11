using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQueryHandler:IRequestHandler<GetUserQuery,ApiResponse<UserDTO>>
{
    private readonly IBlogDbContext  _blogDbContext;

    public GetUserQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<UserDTO>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users.FindAsync(query.UserId) ?? throw new ApiException("User not found with id " + query.UserId, HttpStatusCode.NotFound);
        return new ApiResponse<UserDTO>
        {
            Data = new UserDTO(user),
            Message = "User fetched successfully"
        };
    }
}