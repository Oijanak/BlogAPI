using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using MediatR;

namespace BlogApi.Application.Features.Category.Commands.UpdateCategoryCommand;

public class UpdateCategoryCommandHandler:IRequestHandler<UpdateCategoryCommand, ApiResponse<CategoryDto>>
{
    private readonly IBlogDbContext _blogDbContext;

    public UpdateCategoryCommandHandler(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<ApiResponse<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _blogDbContext.Categories.FindAsync(request.CategoryId);
        existingCategory.CategoryName = request.CategoryName;
        await _blogDbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<CategoryDto>
        {
            Data = new CategoryDto
                { CategotyId = existingCategory.CategoryId, CategoryName = existingCategory.CategoryName },
            Message = "Category updated successfully."
        };

    }
}