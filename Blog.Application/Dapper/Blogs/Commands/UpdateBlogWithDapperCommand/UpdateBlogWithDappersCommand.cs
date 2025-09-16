using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;

public class UpdateBlogWithDappersCommand:IRequest<ApiResponse<BlogDTO>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
    [FromBody]
    public UpdateBlogRequest Blog { get; set; }
}