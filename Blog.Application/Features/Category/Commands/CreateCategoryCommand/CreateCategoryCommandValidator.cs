using FluentValidation;

namespace BlogApi.Application.Features.Category.Commands;

public class CreateCategoryCommandValidator:AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c=>c.CategoryName).NotEmpty().WithMessage("CategoryName is required").MaximumLength(100)
            .WithMessage("CategoryName must not exceed 100 characters");;
    }
}