using System.Data;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Blogs.Commands.DeleteBlogWithDapperCommand;

public class DeleteBlogWithDapperCommandHandler:IRequestHandler<DeleteBlogWithDapperCommand, ApiResponse<string>>
{
    private readonly IDbConnection _dbConnection;

    public DeleteBlogWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<string>> Handle(DeleteBlogWithDapperCommand request, CancellationToken cancellationToken)
    {
        var blog = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Blogs] where BlogId=@BlogId",
            request);
        Guard.Against.Null(blog, nameof(blog),"Blog with Id not found");
        await _dbConnection.ExecuteAsync("delete from [Blogs] where BlogId=@BlogId", request);
        return new ApiResponse<string>
        {
            Message = "Blog deleted successfully"
        };
    }
}