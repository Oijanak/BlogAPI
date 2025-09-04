using FluentValidation;

namespace BlogApi.Application.DTOs.Validators;

public class UpdateUserRequestValidator:AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is Invalid");
        RuleFor(x=>x).Must(x=>!string.IsNullOrWhiteSpace(x.Email)||
                                      !string.IsNullOrWhiteSpace(x.Name)|| !string.IsNullOrWhiteSpace(x.Password))
                    .WithMessage("At least one field is required. ");
    }
    
}