using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.SP.Blogs.Commands.DeleteBlogWithSpCommand;

public class DeleteBlogWithSpCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid BlogId { get;}
    
}