using FluentValidation;

namespace BlogApi.Application.Dapper.Authors.CreateAuthorWithDapperCommand;

public class CreateAuthorWithDapperCommandValidator:AbstractValidator<CreateAuthorWithDapperCommand>
{
    public CreateAuthorWithDapperCommandValidator()
    {
        RuleFor(x => x.AuthorEmail)
            .NotEmpty().WithMessage("Email is Required")
            .EmailAddress().WithMessage("Invalid email address");
        
        RuleFor(x => x.AuthorName).NotEmpty().WithMessage("Author name is Required");
        RuleFor(x => x.Age).GreaterThan(0).WithMessage("Author age should be greater than 0").LessThan(120)
            .WithMessage("Author age should be less than 120 years");
    }
    
}