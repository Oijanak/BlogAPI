using FluentValidation;

namespace BlogApi.Application.DTOs.Validators;

public class UpdateBlogRequestValidator:AbstractValidator<UpdateBlogRequest>
{
    public UpdateBlogRequestValidator()
    {
        RuleFor(x => x.BlogTitle).MaximumLength(200).WithMessage("Blog title cannot exceed 200 characters");
        RuleFor(x => x)
                    .Must(x => !string.IsNullOrWhiteSpace(x.BlogTitle) || !string.IsNullOrWhiteSpace(x.BlogContent))
                    .WithMessage("At least BlogTitle or BlogContent must have a value.");
    }
}