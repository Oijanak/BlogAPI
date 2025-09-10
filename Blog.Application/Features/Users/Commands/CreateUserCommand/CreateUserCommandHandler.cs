using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly BlogDbContext _blogDbContext;
    public CreateUserCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
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
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new UserDTO(user);
    }
}