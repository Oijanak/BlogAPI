using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.SP.Blogs.Commands;

public class CreateBlogWithSpCommand:IRequest<ApiResponse<BlogDTO>>
{
    public Guid AuthorId { get;}
    public string BlogTitle { get; }
    public string BlogContent { get; }

    public CreateBlogWithSpCommand(Guid authorId,string blogTitle, string blogContent)
    {
        AuthorId = authorId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
    
}