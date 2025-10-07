using System.Data;
using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;

public class UpdateBlogWithDappersCommandHandler:IRequestHandler<UpdateBlogWithDappersCommand,ApiResponse<BlogDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly ICurrentUserService _currentUserService;

    public UpdateBlogWithDappersCommandHandler(IDbConnection dbConnection,ICurrentUserService currentUserService)
    {
        _dbConnection = dbConnection;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(UpdateBlogWithDappersCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        var existingBlog = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Blogs] where BlogId=@BlogId",
            new{request.BlogId});
        Guard.Against.Null(existingBlog, nameof(existingBlog),"Blog with Id not found");
        var author = await _dbConnection.QueryFirstOrDefaultAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.Blog.AuthorId });
        Guard.Against.Null(author, nameof(author),"Author with Id not found");
        var blogs=await _dbConnection.QueryAsync<BlogDTO,UserDto,UserDto,AuthorDto,BlogDTO>(
            "spUpdateBlog",
            (blog, createdBy, updatedBy, author) =>
            {
                blog.CreatedBy = createdBy;
                blog.UpdatedBy = updatedBy;
                blog.Author = author;
                return blog;

            },
            new {request.BlogId,request.Blog.BlogTitle,request.Blog.BlogContent,request.Blog.StartDate,request.Blog.EndDate,request.Blog.AuthorId,UpdatedBy = currentUserId},
            splitOn: "Id,Id,AuthorId",
            commandType:CommandType.StoredProcedure
            );
        var blog = blogs.FirstOrDefault();
        return new ApiResponse<BlogDTO>
        {
            Data = blog,
            Message = "Blog updated successfully"
        };
    }
}