using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommand:IRequest<BlogDTO>
{
    public int UserId { get;}
    public string BlogTitle { get; }
    public string BlogContent { get; }

    public CreateBlogCommand(int userId,string blogTitle, string blogContent)
    {
        UserId = userId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
}