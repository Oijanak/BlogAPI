using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommand:IRequest<BlogDTO>
{
    public int BlogId { get; set; }
    public int AuthorId { get; set; }
    public string BlogTitle { get;}
    public string BlogContent { get;}

    public UpdateBlogCommand(int blogId, int authorId, string blogTitle, string blogContent)
    {
        BlogId = blogId;    
        AuthorId = authorId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
    
}