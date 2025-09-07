using FluentValidation;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQueryValidator:AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is Required");

    }
    
}