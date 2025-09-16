using FluentValidation;

namespace BlogApi.Application.Dapper.Users.Commands.UpdateUserWithDapperCommand;

public class UpdateUserWithDapperCommandValidator:AbstractValidator<UpdateUserWithDapperCommand>
{
    public UpdateUserWithDapperCommandValidator()
    {
        RuleFor(x=>x.UserId).NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.User.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.User.Email)).WithMessage("Email is null");

        RuleFor(x => x.User.Name)
            .NotEmpty().WithMessage("Name is required");
        
        RuleFor(x => x.User.Password)
            .NotEmpty().WithMessage("Password is required");


    }
}