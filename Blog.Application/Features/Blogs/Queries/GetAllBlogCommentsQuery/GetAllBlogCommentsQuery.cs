using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllBlogCommentsQuery;

public class GetAllBlogCommentsQuery:IRequest<ApiResponse<IEnumerable<CommentDto>>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
    
}