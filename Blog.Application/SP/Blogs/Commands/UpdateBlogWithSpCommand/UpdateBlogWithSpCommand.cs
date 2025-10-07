using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;

public class UpdateBlogWithSpCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
    [FromBody]
    public UpdateBlogRequest Blog { get; set; }
    
   
}