using System.Net;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetBlogsByUserId;

public class GetBlogsByUserIdQueryHadler:IRequestHandler<GetBlogsByUserIdQuery,IEnumerable<BlogDTO>>
{
    private readonly IUserRepository _userRepository;

    public GetBlogsByUserIdQueryHadler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IEnumerable<BlogDTO>> Handle(GetBlogsByUserIdQuery request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new ApiException("User not found with id " + request.UserId, HttpStatusCode.NotFound);
        return user.Blogs.Select(b => new BlogDTO
        {
            BlogId = b.BlogId,
            BlogTitle = b.BlogTitle,
            BlogContent = b.BlogContent,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt,
        });
    }
}