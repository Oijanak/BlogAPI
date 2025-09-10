using MediatR;

namespace BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;

public class DeleteBlogWithSpCommand:IRequest<Unit>
{
    public Guid BlogId { get;}
    

    public DeleteBlogWithSpCommand(Guid blogId)
    {
        BlogId = blogId;
       
    }
}