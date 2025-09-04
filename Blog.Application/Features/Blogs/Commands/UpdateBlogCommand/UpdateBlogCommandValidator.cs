using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.UpdateBlogCommand;

public class UpdateBlogCommandValidator:AbstractValidator<UpdateBlogCommand>
{
    public UpdateBlogCommandValidator()
    {
        RuleFor(x => x.BlogId).NotEmpty().GreaterThan(0).WithMessage("Blog Id is Required");
        RuleFor(x => x.BlogTitle).MaximumLength(200).WithMessage("Blog Title should be of length 200 characters");
        RuleFor(x=>x.UserId).NotEmpty().WithMessage("User Id is Required").GreaterThan(0).WithMessage("User Id is Invalid");
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.BlogTitle) || !string.IsNullOrWhiteSpace(x.BlogContent))
            .WithMessage("At least BlogTitle or BlogContent must have a value.");
    }
}