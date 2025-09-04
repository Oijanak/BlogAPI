using FluentValidation;

namespace BlogApi.Application.Features.Users.Query.LoginUserRequest;

public class LoginUserRequestValidation: AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidation()
    {
        RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid Email");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

    }
    
}