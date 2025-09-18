using System.Data;
using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;

public class CreateBlogWithDapperCommandHandler:IRequestHandler<CreateBlogWithDapperCommand,ApiResponse<BlogDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly string _currentUserId;
    public CreateBlogWithDapperCommandHandler(IDbConnection dbConnection,IHttpContextAccessor httpContextAccessor)
    {
        _dbConnection = dbConnection;
        _currentUserId=httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(CreateBlogWithDapperCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbConnection.QueryFirstAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.AuthorId });
        Guard.Against.Null(author, nameof(author),"Author with Id not found");
        var blogs = await _dbConnection.QueryAsync<BlogDTO, AuthorDto, BlogDTO>(
            "spCreateBlogWithAuthor",
            (blog, author) =>
            {
                blog.Author = author;
                return blog;
            },
            new{request.AuthorId,request.BlogTitle,request.BlogContent,CreatedBy=_currentUserId},
            splitOn: "AuthorId",   
            commandType: CommandType.StoredProcedure
        );
        var blog = blogs.FirstOrDefault();
        return new ApiResponse<BlogDTO>
        {
            Data = blog,
            Message = "Blog created successfully"
        };
    }
}