using System.Data;
using Ardalis.GuardClauses;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;

public class CreateBlogWithDapperCommandHandler:IRequestHandler<CreateBlogWithDapperCommand,ApiResponse<BlogDTO>>
{
    private readonly IDbConnection _dbConnection;

    public CreateBlogWithDapperCommandHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
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
            request,
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