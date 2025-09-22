using FluentValidation;

namespace BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;

public class RegisterUserCommandValidator:AbstractValidator<RegisterUserCommand>
{
    public  RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
    
}