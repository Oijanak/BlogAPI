
using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Blogs.Commands.ApproveStatusCommand;

public class ApproveStatusCommand : IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid BlogId { get; set; }
}