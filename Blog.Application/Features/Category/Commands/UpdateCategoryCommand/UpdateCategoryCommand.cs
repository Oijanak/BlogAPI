using BlogApi.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Application.Features.Category.Commands.UpdateCategoryCommand;

public class UpdateCategoryCommand:IRequest<ApiResponse<CategoryDto>>
{
    [FromRoute]
    public Guid CategoryId { get; set; }
    [FromBody]
    public string CategoryName { get; set; }
    
}