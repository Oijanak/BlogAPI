using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Query.GetUserListQuery;

public class GetUserListQueryHandler:IRequestHandler<GetUserListQuery, IEnumerable<UserDTO>>
{
    private readonly IUserRepository _userRepository;

    public GetUserListQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IEnumerable<UserDTO>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserDTO
        {
            UserId = u.UserId,
            Name = u.Name,
            Email = u.Email,
            Blogs = u.Blogs.Select(b => new BlogDTO
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogContent = b.BlogContent,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
            }).ToList()
        });
    }
}