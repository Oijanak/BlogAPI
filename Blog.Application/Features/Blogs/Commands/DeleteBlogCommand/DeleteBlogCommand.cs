using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommand: IRequest<Unit>
{
    public Guid BlogId { get;}
    

    public DeleteBlogCommand(Guid blogId)
    {
        BlogId = blogId;
       
    }
}