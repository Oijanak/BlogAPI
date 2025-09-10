using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;

public class UpdateBlogWithSpCommand:IRequest<BlogDTO>
{
    public Guid BlogId { get; }
    public Guid AuthorId { get;}
    public string BlogTitle { get; }
    public string BlogContent { get; }
    
    public UpdateBlogWithSpCommand(Guid blogId,Guid authorId,string blogTitle, string blogContent)
    {
        BlogId = blogId;
        AuthorId = authorId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
}