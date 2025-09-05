using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly BlogDbContext _blogDbContext;
    public CreateUserCommandHandler(BlogDbContext _blogDbContext)
    {
        _blogDbContext = _blogDbContext;
    }

    public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        await _blogDbContext.Users.AddAsync(user);
        return new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email
        };
    }
}