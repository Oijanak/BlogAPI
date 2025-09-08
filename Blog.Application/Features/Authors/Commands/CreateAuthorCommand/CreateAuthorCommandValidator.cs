using BlogApi.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;

public class CreateAuthorCommandValidator:AbstractValidator<CreateAuthorCommand>
{
    private  readonly BlogDbContext _blogDbContext; 
    public CreateAuthorCommandValidator(BlogDbContext blogDbContext)
    {
        this._blogDbContext = blogDbContext;
        RuleFor(x => x.AuthorEmail)
            .NotEmpty().WithMessage("Email is Required")
            .EmailAddress().WithMessage("Invalid email address")
            .MustAsync(async (email, cancellation) =>
                !await _blogDbContext.Authors.AnyAsync(a => a.AuthorEmail == email, cancellation))
            .WithMessage("Email already exists");
        RuleFor(x => x.AuthorName).NotEmpty().WithMessage("Author name is Required");
        RuleFor(x => x.Age).GreaterThan(0).WithMessage("Author age should be greater than 0").LessThan(120)
            .WithMessage("Author age should be less than 120 years");
    }
}