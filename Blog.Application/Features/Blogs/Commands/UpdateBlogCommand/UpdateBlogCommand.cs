using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommand:IRequest<ApiResponse<BlogDTO>>
{
    public Guid BlogId { get; set; }
    public Guid AuthorId { get; set; }
    public string BlogTitle { get;}
    public string BlogContent { get;}

    public UpdateBlogCommand(Guid blogId, Guid authorId, string blogTitle, string blogContent)
    {
        BlogId = blogId;    
        AuthorId = authorId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
    
}