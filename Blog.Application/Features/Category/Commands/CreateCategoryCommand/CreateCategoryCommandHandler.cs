using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using MediatR;

namespace BlogApi.Application.Features.Category.Commands
{

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse<CategoryDto>>
    {
        private readonly IBlogDbContext _blogDbContext;

        public CreateCategoryCommandHandler(IBlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<ApiResponse<CategoryDto>> Handle(CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            Domain.Models.Category category = new Domain.Models.Category
            {
                CategoryName = request.CategoryName,
            };
            await _blogDbContext.Categories.AddAsync(category, cancellationToken);
            await _blogDbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse<CategoryDto>
            {
                Data = new CategoryDto { CategotyId = category.CategoryId, CategoryName = category.CategoryName },
                Message = "Category created successfully"
            };

        }
    }
}