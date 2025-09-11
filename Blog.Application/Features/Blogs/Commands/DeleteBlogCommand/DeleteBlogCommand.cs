using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommand: IRequest<ApiResponse<string>>
{
    public Guid BlogId { get;}
    

    public DeleteBlogCommand(Guid blogId)
    {
        BlogId = blogId;
       
    }
}