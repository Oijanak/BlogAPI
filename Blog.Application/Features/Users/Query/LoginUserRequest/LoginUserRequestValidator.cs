using FluentValidation;

namespace BlogApi.Application.Features.Users.Query.LoginUserRequest;

public class LoginUserRequestValidator: AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid Email");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

    }
    
}