using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using MediatR;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,Unit>
{
    private readonly BlogDbContext _blogDbContext;

    public DeleteUserCommandHandler(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users.FindAsync(request.UserId) ?? throw new ApiException("User not found with id " + request.UserId, HttpStatusCode.NotFound);
        _blogDbContext.Users.Remove(user);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}