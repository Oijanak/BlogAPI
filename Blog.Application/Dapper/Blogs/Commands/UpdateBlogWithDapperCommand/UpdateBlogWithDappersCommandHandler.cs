using System.Data;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;

public class UpdateBlogWithDappersCommandHandler:IRequestHandler<UpdateBlogWithDappersCommand,ApiResponse<BlogDTO>>
{
    private readonly IDbConnection _dbConnection;

    public UpdateBlogWithDappersCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(UpdateBlogWithDappersCommand request, CancellationToken cancellationToken)
    {
        var existingBlog = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Blogs] where BlogId=@BlogId",
            new{request.BlogId});
        Guard.Against.Null(existingBlog, nameof(existingBlog),"Blog with Id not found");
        var author = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.Blog.AuthorId });
        Guard.Against.Null(author, nameof(author),"Author with Id not found");
        var blog=await _dbConnection.QueryFirstAsync<BlogDTO>("spUpdateBlog", new {request.BlogId,request.Blog.BlogTitle,request.Blog.BlogContent,request.Blog.AuthorId},commandType:CommandType.StoredProcedure);
        return new ApiResponse<BlogDTO>
        {
            Data = blog,
            Message = "Blog updated successfully"
        };
    }
}