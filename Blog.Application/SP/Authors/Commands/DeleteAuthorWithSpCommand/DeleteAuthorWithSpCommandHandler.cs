using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Authors.Commands.DeleteAuthorWithSpCommand;

public class DeleteAuthorWithSpCommandHandler:IRequestHandler<DeleteAuthorWithSpCommand, ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteAuthorWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteAuthorWithSpCommand request, CancellationToken cancellationToken)
    {
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spDeleteAuthor {request.AuthorId}");
        return new ApiResponse<string>
        {
            Message = "Author deleted successfully",
        };
    }
}