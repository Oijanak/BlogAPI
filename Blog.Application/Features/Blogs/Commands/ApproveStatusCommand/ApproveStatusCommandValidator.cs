using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.ApproveStatusCommand;

public class ApproveStatusCommandValidator:AbstractValidator<ApproveStatusCommand>
{
    public ApproveStatusCommandValidator()
    {
        RuleFor(request => request.BlogId).NotNull().WithMessage("BlogId cannot be null");
    }
}