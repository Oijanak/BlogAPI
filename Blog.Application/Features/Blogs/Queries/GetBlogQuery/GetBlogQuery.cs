using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQuery:IRequest<ApiResponse<BlogDTO>>
{
    [FromRoute]
    public Guid BlogId { get;}

    public GetBlogQuery(Guid blogId)
    {
        BlogId = blogId;
    }
    
    
}