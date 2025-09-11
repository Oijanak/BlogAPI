using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;

public class DeleteUserWithSpCommandHandler:IRequestHandler<DeleteUserWithSpCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteUserWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteUserWithSpCommand request, CancellationToken cancellationToken)
    {
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spDeleteBlog {request.UserId}");
        return new ApiResponse<string>
        {
            Message = "User deleted successfully",
        };

    }
}