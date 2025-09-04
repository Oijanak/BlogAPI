using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQuery:IRequest<BlogDTO>
{
    public int BlogId { get;}

    public GetBlogQuery(int blogId)
    {
        BlogId = blogId;
    }
    
    
}