using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQueryHandler:IRequestHandler<GetUserQuery,UserDTO>
{
    private readonly IUserRepository  _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository=userRepository;
    }
    public async Task<UserDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(query.UserId) ?? throw new ApiException("User not found with id " + query.UserId, HttpStatusCode.NotFound);
        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Blogs = user.Blogs.Select(b => new BlogDTO
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogContent = b.BlogContent,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
            }).ToList()
        }; 
    }
}