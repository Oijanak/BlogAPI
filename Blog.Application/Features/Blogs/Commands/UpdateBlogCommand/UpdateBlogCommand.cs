using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommand:IRequest<ApiResponse<BlogDTO>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
    [FromForm]
    public UpdateBlogRequest Blog { get; set; }
    
}