using System.Net;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Exceptions;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteUserCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _blogDbContext.Users.FindAsync(request.UserId);
        Guard.Against.Null(user,nameof(user),"User cannot be null");
        _blogDbContext.Users.Remove(user);
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<string>
        {
            Message = "User deleted successfully",
        };
    }
}