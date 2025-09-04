using FluentValidation;

namespace BlogApi.Application.Features.Users.Query.GetBlogsByUserId;

public class GetBlogsByUserIdQueryValidator:AbstractValidator<GetBlogsByUserIdQuery>
{
    public GetBlogsByUserIdQueryValidator()
    {
        RuleFor(x=>x.UserId).NotEmpty().WithMessage("UserId is required.")
            .GreaterThan(0).WithMessage("UserId is invalid.");
    }
}