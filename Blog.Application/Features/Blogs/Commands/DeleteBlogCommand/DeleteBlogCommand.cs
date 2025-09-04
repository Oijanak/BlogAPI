using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommand: IRequest<Unit>
{
    public int BlogId { get;}
    public int UserId { get;}

    public DeleteBlogCommand(int blogId, int userId)
    {
        BlogId = blogId;
        UserId = userId;
    }
}