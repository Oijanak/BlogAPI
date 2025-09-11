using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;

public class DeleteBlogWithSpCommandHandler:IRequestHandler<DeleteBlogWithSpCommand,ApiResponse<string>>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteBlogWithSpCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<string>> Handle(DeleteBlogWithSpCommand request, CancellationToken cancellationToken)
    {
        await _blogDbContext.Database
            .ExecuteSqlInterpolatedAsync($"EXEC spDeleteUser {request.BlogId}");
        return new ApiResponse<string>
        {
            Message = "Blog deleted successfully",
        };
    }
}