using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommand: IRequest<Unit>
{
    public int BlogId { get;}
    

    public DeleteBlogCommand(int blogId)
    {
        BlogId = blogId;
       
    }
}