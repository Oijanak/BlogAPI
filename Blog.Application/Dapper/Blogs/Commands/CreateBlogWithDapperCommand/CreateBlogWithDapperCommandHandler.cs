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
    private readonly IFileService _fileService;
    public CreateBlogWithDapperCommandHandler(IDbConnection dbConnection,ICurrentUserService currentUserService,IFileService fileService)
    {
        _dbConnection = dbConnection;
        _currentUserService = currentUserService;
        _fileService=fileService;
    }
    public async Task<ApiResponse<BlogDTO>> Handle(CreateBlogWithDapperCommand request, CancellationToken cancellationToken)
    {
        var currentUserId=_currentUserService.UserId;
        var categoryIds=string.Empty;
        if(request.Categories!=null && request.Categories.Count>0)
            categoryIds=string.Join(',',request.Categories);
        var author = await _dbConnection.QueryFirstAsync<AuthorDto>("select * from [Authors] where AuthorId=@AuthorId",
            new { request.AuthorId });
        Guard.Against.Null(author, nameof(author),"Author with Id not found");
        var documents = await _fileService.UploadFilesAsync(request.Files);
        var result = await _dbConnection.QueryMultipleAsync(
            "spCreateBlog",
            new{request.AuthorId,request.BlogTitle,request.BlogContent,request.StartDate,request.EndDate,CreatedBy=currentUserId,categoryIds},
            commandType: CommandType.StoredProcedure
        );
        
        var blogs = result.Read<BlogDTO, UserDto, AuthorDto,BlogDTO>(
            (blog, createdBy, authors) =>
            {
                blog.CreatedBy = createdBy;
                blog.Author = authors;
                
                return blog;
            },
            splitOn: "Id,AuthorId"
        ).ToList();
        var blog=blogs.FirstOrDefault();
      

        var categories = (await result.ReadAsync<CategoryDto>()).ToList();
        blog.Categories = categories;
        if (documents != null && documents.Any())
        {
            foreach (var doc in documents)
            {
                doc.BlogId = blog.BlogId; 
                await _dbConnection.ExecuteAsync(
                    "INSERT INTO BlogDocument (BlogDocumentId, DocumentName, DocumentPath, DocumentType, DocumentSize, BlogId) " +
                    "VALUES (@BlogDocumentId, @DocumentName, @DocumentPath, @DocumentType, @DocumentSize, @BlogId)",
                    doc
                );
            }
        }
        blog.BlogDocuments=documents.Select(d=>new BlogDocumentDto{DocumentName = d.DocumentName,BlogDocumentId = d.BlogDocumentId,DocumentType = d.DocumentType}).ToList();
        return new ApiResponse<BlogDTO>
        {
            Data = blog,
            Message = "Blog created successfully"
        };
    }
    
    
}