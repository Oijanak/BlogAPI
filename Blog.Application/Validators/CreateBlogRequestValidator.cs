using FluentValidation;

namespace BlogApi.Application.DTOs.Validators;

public class CreateBlogRequestValidator:AbstractValidator<CreateBlogRequest>
{
    public CreateBlogRequestValidator()
    {
        RuleFor(x=>x.BlogTitle).NotEmpty().WithMessage("Blog title is required")
            .MaximumLength(200).WithMessage("Blog title cannot exceed 200 characters");
        RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog content is required");
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is required");
    }
    
}