using BlogApi.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace BlogApi.Application.Features.Category.Commands.UpdateCategoryCommand;

public class UpdateCategoryCommandValidator:AbstractValidator<UpdateCategoryCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    
    public UpdateCategoryCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(c=>c.CategoryId).NotEmpty().WithMessage("CategoryId is required")
            .MustAsync(async (categoryId, cancellationToken) =>
                await _blogDbContext.Categories.FindAsync(categoryId) != null)
            .WithMessage("Category not found").WithErrorCode("404");;;
        RuleFor(c=>c.CategoryName).NotEmpty().WithMessage("CategoryName is required")
            .MaximumLength(100).WithMessage("CategoryName must not exceed 100 characters");
        
    }
    
}