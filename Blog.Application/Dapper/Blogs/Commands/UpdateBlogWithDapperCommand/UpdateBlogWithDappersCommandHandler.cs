using System.Data;
using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;

public class UpdateBlogWithDappersCommandHandler:IRequestHandler<UpdateBlogWithDappersCommand,ApiResponse<BlogDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly string _currentUserId;

    public UpdateBlogWithDappersCommandHandler(IDbConnection dbConnection,IHttpContextAccessor httpContextAccessor)
    {
        _dbConnection = dbConnection;
        _currentUserId=httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(UpdateBlogWithDappersCommand request, CancellationToken cancellationToken)
    {
        var existingBlog = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Blogs] where BlogId=@BlogId",
            new{request.BlogId});
        Guard.Against.Null(existingBlog, nameof(existingBlog),"Blog with Id not found");
        var author = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.Blog.AuthorId });
        Guard.Against.Null(author, nameof(author),"Author with Id not found");
        var blog=await _dbConnection.QueryFirstAsync<BlogDTO>("spUpdateBlog", new {request.BlogId,request.Blog.BlogTitle,request.Blog.BlogContent,request.Blog.AuthorId,UpdatedBy = _currentUserId},commandType:CommandType.StoredProcedure);
        return new ApiResponse<BlogDTO>
        {
            Data = blog,
            Message = "Blog updated successfully"
        };
    }
}