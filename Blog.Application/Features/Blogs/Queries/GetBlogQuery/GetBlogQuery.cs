using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQuery:IRequest<ApiResponse<BlogDTO>>
{
    public Guid BlogId { get;}

    public GetBlogQuery(Guid blogId)
    {
        BlogId = blogId;
    }
    
    
}