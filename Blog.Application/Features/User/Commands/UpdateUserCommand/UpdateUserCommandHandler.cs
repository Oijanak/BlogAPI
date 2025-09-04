using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

public class UpdateUserCommanHanler:IRequestHandler<UpdateUserCommand,UserDTO>
{
    private readonly IUserRepository _userRepository;
    public UpdateUserCommanHanler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(request.UserId)??
            throw new ApiException("User not found with id " + request.UserId, System.Net.HttpStatusCode.NotFound);;
        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        if (request.Password is not null)
            user.Password = request.Password;

        User updatedUser = await _userRepository.Update(user);

        return new UserDTO
        {
            UserId = updatedUser.UserId,
            Name = updatedUser.Name,
            Email = updatedUser.Email
        };
    }
}