using BlogApi.Application.DTOs;
using MediatR;

namespace BlogApi.Application.Features.Category.Queries.GetAllCategoryQuery;

public class GetAllCategoryQuery:IRequest<ApiResponse<IEnumerable<CategoryDto>>>
{
    
}