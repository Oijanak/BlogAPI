using System.Data;
using System.Security.Claims;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;

public class CreateBlogWithDapperCommandHandler:IRequestHandler<CreateBlogWithDapperCommand,ApiResponse<BlogDTO>>
{
    private readonly IDbConnection _dbConnection;
    private readonly ICurrentUserService _currentUserService;
    public CreateBlogWithDapperCommandHandler(IDbConnection dbConnection,ICurrentUserService currentUserService)
    {
        _dbConnection = dbConnection;
        _currentUserService = currentUserService;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(CreateBlogWithDapperCommand request, CancellationToken cancellationToken)
    {
        var currentUserId=_currentUserService.UserId;
        var categoryIds = string.Join(',',request.Categories);
        var author = await _dbConnection.QueryFirstAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.AuthorId });
        Guard.Against.Null(author, nameof(author),"Author with Id not found");
        var result = await _dbConnection.QueryMultipleAsync(
            "spCreateBlog",
            new{request.AuthorId,request.BlogTitle,request.BlogContent,request.StartDate,request.EndDate,CreatedBy=currentUserId,categoryIds},
            commandType: CommandType.StoredProcedure
        );
        
        var blogs = result.Read<BlogDTO, AuthorDto, UserDto,BlogDTO>(
            (blog, authors, createdBy) =>
            {
                blog.Author = authors;
                blog.CreatedBy = createdBy;
                return blog;
            },
            splitOn: "Id,AuthorId"
        ).ToList();
        var blog=blogs.FirstOrDefault();
        var categories = (await result.ReadAsync<CategoryDto>()).ToList();
        blog.Categories = categories;
        return new ApiResponse<BlogDTO>
        {
            Data = blog,
            Message = "Blog created successfully"
        };
    }
}