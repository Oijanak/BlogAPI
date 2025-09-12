using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommand: IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
    
}