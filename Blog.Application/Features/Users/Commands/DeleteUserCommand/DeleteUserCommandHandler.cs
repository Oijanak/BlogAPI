using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new ApiException("User not found with id " + request.UserId, HttpStatusCode.NotFound);
        await _userRepository.Delete(user);
        return Unit.Value;
    }
}