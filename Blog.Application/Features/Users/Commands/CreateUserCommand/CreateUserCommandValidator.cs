using BlogApi.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly BlogDbContext _blogDbContext;

   
    public CreateUserCommandValidator(BlogDbContext blogDbContext)
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