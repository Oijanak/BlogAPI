using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Category.Commands;

public class DeleteCategoryCommand:IRequest<ApiResponse<string>>
{
    [FromRoute]
    public Guid CategoryId { get; set; }
    
}