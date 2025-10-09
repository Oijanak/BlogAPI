using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Category.Queries.GetAllCategoryQuery;

public class GetAllCategoryQueryHandler:IRequestHandler<GetAllCategoryQuery,ApiResponse<IEnumerable<CategoryDto>>>
{
    private readonly IBlogDbContext _blogDbContext;

    public GetAllCategoryQueryHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<IEnumerable<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _blogDbContext.Categories.Select(c=>new CategoryDto{CategotyId = c.CategoryId,CategoryName = c.CategoryName}).ToListAsync();
        return new ApiResponse<IEnumerable<CategoryDto>>
        {
            Data = categories,
            Message = "Blog fetched Successfully",
        };

    }
}