using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly IUserRepository _userRepository;
    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
        User createdUser = await _userRepository.AddAsync(user);
        return new UserDTO
        {
            UserId = createdUser.UserId,
            Name = createdUser.Name,
            Email = createdUser.Email
        };
    }
}