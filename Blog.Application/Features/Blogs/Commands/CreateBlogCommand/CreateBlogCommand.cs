using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommand:IRequest<BlogDTO>
{
    public Guid AuthorId { get;}
    public string BlogTitle { get; }
    public string BlogContent { get; }

    public CreateBlogCommand(Guid authorId,string blogTitle, string blogContent)
    {
        AuthorId = authorId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
}