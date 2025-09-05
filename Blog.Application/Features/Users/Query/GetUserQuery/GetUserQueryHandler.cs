using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQueryHandler:IRequestHandler<GetUserQuery,UserDTO>
{
    private readonly BlogDbContext  _blogDbContext;

    public GetUserQueryHandler(BlogDbContext _blogDbContext)
    {
        _blogDbContext = this._blogDbContext;
    }
    public async Task<UserDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users.FindAsync(query.UserId) ?? throw new ApiException("User not found with id " + query.UserId, HttpStatusCode.NotFound);
        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email
        }; 
    }
}