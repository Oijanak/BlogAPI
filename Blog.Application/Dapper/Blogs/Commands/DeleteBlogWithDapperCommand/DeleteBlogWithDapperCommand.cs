using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Dapper.Blogs.Commands.DeleteBlogWithDapperCommand;

public class DeleteBlogWithDapperCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
}