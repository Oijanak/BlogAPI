using BlogApi.Application.DTOs;
using FluentValidation;

namespace BlogApi.Application.Dapper.Authors.UpdateAuthorWithDapperCommand;

public class UpdateAuthorWithDapperValidator:AbstractValidator<UpdateAuthorWithDapperCommand>
{
    public UpdateAuthorWithDapperValidator()
    {
        RuleFor(x=>x.AuthorId).NotEmpty().WithMessage("AuthorId is required");;
        RuleFor(x => x.Author.AuthorEmail)
            .NotEmpty().WithMessage("Email is Required")
            .EmailAddress().WithMessage("Invalid email address");
        RuleFor(x => x.Author.AuthorName).NotEmpty().WithMessage("Author name is Required");
        RuleFor(x => x.Author.Age).GreaterThan(0).WithMessage("Author age should be greater than 0").LessThan(120)
            .WithMessage("Author age should be less than 120 years");
    }
}