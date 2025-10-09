using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Category.Commands;

public class CreateCategoryCommand:IRequest<ApiResponse<CategoryDto>>
{
    public string CategoryName { get; set; }
}