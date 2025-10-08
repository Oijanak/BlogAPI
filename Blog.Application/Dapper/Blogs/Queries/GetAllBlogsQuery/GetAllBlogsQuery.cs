using BlogApi.Application.DTOs;
using BlogApi.Domain.Enum;
using MediatR;

namespace BlogApi.Application.Dapper.Blogs.Queries;

public record GetAllBlogsQuery(
   
    int Page = 1,
    
    int Limit = 10,
    
    string? SortBy  = "CreatedAt",
    string? SortOrder = "desc",
    
    DateTime? StartDate = null,
    
    DateTime ? EndDate = null,
    
    string? CreatedBy = null,
    
    string? ApprovedBy = null,
    
    ApproveStatus? ApproveStatus = null,
    
    ActiveStatus? ActiveStatus = null
) : IRequest<ApiResponse<IEnumerable<BlogDTO>>>;