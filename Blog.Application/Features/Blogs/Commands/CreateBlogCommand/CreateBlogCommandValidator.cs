using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommandValidator: AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required").GreaterThan(0).WithMessage("User Id is Invalid");
        RuleFor(x => x.BlogTitle).NotEmpty().WithMessage("Blog Title is Required").MaximumLength(200).WithMessage("Blog Title Maximum length is 200 characters");
        RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog Content is Required");
    }
    
}