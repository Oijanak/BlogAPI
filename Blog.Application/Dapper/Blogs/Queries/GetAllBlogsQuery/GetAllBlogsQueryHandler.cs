using System.Data;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Models;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Blogs.Queries;

public class GetAllBlogsQueryHandler:IRequestHandler<GetAllBlogsQuery,ApiResponse<IEnumerable<BlogDTO>>>
{
    private readonly IDbConnection _dbConnection;

    public GetAllBlogsQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public async Task<ApiResponse<IEnumerable<BlogDTO>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
       
        var result = await _dbConnection.QueryMultipleAsync(
            "spGetBlogList",
            new {
                request.StartDate,
                request.EndDate,
                request.CreatedBy,
                request.ApprovedBy,
                request.ApproveStatus,
                request.ActiveStatus,
                request.SortBy,
                request.SortOrder,
                request.Page,
                request.Limit
            },
            commandType: CommandType.StoredProcedure
        );

        // 1️⃣ Read blogs with author and user info
        var blogs = result.Read<BlogDTO, AuthorDto, UserDto, UserDto, UserDto, BlogDTO>(
            (blog, author, createdBy, updatedBy, approveBy) =>
            {
                blog.Author = author;
                blog.CreatedBy = createdBy;
                blog.UpdatedBy = updatedBy;
                blog.ApprovedBy = approveBy;
                return blog;
            },
            splitOn: "AuthorId,Id,Id,Id"
        ).ToList();

     
        var documents = result.Read<BlogDocument>().ToList();

        var categoriesRaw = result.Read<(Guid CategoryId, string CategoryName, Guid BlogsBlogId)>().ToList();

        // 4️⃣ Assign documents and categories to blogs
        foreach (var blog in blogs)
        {
            blog.BlogDocuments = documents.Where(d => d.BlogId == blog.BlogId).Select(d=>new BlogDocumentDto{BlogDocumentId = d.BlogDocumentId,DocumentName = d.DocumentName,DocumentType = d.DocumentType}).ToList();
            blog.Categories = categoriesRaw
                .Where(c => c.BlogsBlogId == blog.BlogId)
                .Select(c => new CategoryDto 
                { 
                    CategotyId = c.CategoryId, 
                    CategoryName = c.CategoryName 
                })
                .ToList();
        }

        // 5️⃣ Read total count
        var totalCount = result.ReadSingle<int>();

        return new ApiResponse<IEnumerable<BlogDTO>>()
        {
            Data = blogs,
            totalSize = totalCount,
            Message = "Blogs fetched successfully"
        };
        }
    }
