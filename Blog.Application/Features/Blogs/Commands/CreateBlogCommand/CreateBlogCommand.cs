using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommand:IRequest<ApiResponse<BlogDTO>>
{
    public Guid AuthorId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogContent { get; set; }
    public CreateBlogCommand(){}

    public CreateBlogCommand(Guid authorId,string blogTitle, string blogContent)
    {
        AuthorId = authorId;
        BlogTitle = blogTitle;
        BlogContent = blogContent;
    }
}