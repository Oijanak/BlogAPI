using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommand:IRequest<BlogDTO>
{
    public int BlogId { get; set; }
    public int UserId { get; set; }
    public string BlogTitle { get;}
    public string BlogContent { get;}

    public UpdateBlogCommand(int blogId, int userId, string blogTitle, string blogContent)
    {
        BlogId = blogId;    
        UserId = userId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
    
}