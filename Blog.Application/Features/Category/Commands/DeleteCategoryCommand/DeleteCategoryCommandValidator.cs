using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Category.Commands;

public class DeleteCategoryCommandValidator: AbstractValidator<DeleteCategoryCommand>
{
    private readonly IBlogDbContext _blogDbContext;

    public DeleteCategoryCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(c=>c.CategoryId).NotEmpty().WithMessage("CategoryId is required")
            .MustAsync(async (categoryId, cancellationToken) =>
                await _blogDbContext.Categories.FindAsync(categoryId) != null)
            .WithMessage("Category not found").WithErrorCode("404");
    }
    
}