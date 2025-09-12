using BlogApi.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands;

public class CreateUserWithSpCommandValidator:AbstractValidator<CreateUserWithSpCommand>
{
    private readonly IBlogDbContext _blogDbContext;

   
    public CreateUserWithSpCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(async (email, cancellation) =>
                !await _blogDbContext.Users.AnyAsync(u => u.Email == email, cancellation))
            .WithMessage("Email already exists");
        ;

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
    
}