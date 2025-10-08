using System.Data;
using BlogApi.Application.DTOs;
using Dapper;
using MediatR;

namespace BlogApi.Application.Dapper.Blogs.Queries;

public class GetAllBlogsQueryHandler:IRequestHandler<GetAllBlogsQuery,ApiResponse<IEnumerable<BlogDTO>>>
{
    private readonly IDbConnection _dbConnection;

    public GetAllBlogsQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection   = dbConnection;
    }
    public async Task<ApiResponse<IEnumerable<BlogDTO>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        var blogs=await _dbConnection.QueryAsync<BlogDTO,AuthorDto,UserDto,UserDto,UserDto,BlogDTO>(
            "spGetBlogList",
            (blog,author, createdBy, updatedBy,approveBy) =>
            {
                blog.Author = author;
                blog.CreatedBy = createdBy;
                blog.UpdatedBy = updatedBy;
                blog.ApprovedBy = approveBy;
                return blog;

            },
            new {request.StartDate,request.EndDate,request.CreatedBy,request.ApprovedBy,request.ApproveStatus,request.ActiveStatus,request.SortBy,request.SortOrder,request.Page,request.Limit},
            splitOn: "AuthorId,Id,Id,Id",
            commandType:CommandType.StoredProcedure
        );
         
        return new ApiResponse<IEnumerable<BlogDTO>>()
        {
            Data = blogs,
            Message = "Blogs fetched successfully"
        };
    }
}