using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.DeleteBlogCommand;

public class DeleteBlogCommandValidator: AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("BlogId is Required").GreaterThan(0).WithMessage("BlogId is not valid");

    }
    
}