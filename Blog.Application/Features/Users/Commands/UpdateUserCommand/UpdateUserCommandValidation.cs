using FluentValidation;

public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x=>x).Must(x=>!string.IsNullOrWhiteSpace(x.Email)||
                              !string.IsNullOrWhiteSpace(x.Name)|| !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("At least one field is required. ");
    }
    
}