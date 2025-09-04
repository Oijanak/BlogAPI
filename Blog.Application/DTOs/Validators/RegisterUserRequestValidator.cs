using FluentValidation;

namespace BlogApi.Application.DTOs.Validators;

public class RegisterUserRequestValidator:AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is required");
          
    }
    
}