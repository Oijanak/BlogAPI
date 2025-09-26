using BlogApi.Application.DTOs;
using BlogApi.Domain.Enum;
using BlogApi.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;


public record GetBlogListQuery(
   
    int Page = 1,
    
    int Limit = 10,

    string? Search=null,

    string? SortBy  = "CreatedAt",
    string? SortOrder = "desc",
    
    DateTime? StartDate = null,
    
    DateTime ? EndDate = null,
    
    string? CreatedBy = null,
    
    string? ApprovedBy = null,
    
    ApproveStatus? ApproveStatus = null,
    
    ActiveStatus? ActiveStatus = null
) : IRequest<ApiResponse<IEnumerable<BlogDTO>>>;