using BlogApi.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.SP.Users.Commands.UpdateUserWithSpCommand;

public class UpdateUserWithSpCommandValidator:AbstractValidator<UpdateUserWithSpCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public UpdateUserWithSpCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.UserId).NotEmpty()
            .WithMessage("UserId is required").MustAsync(async (userId, cancellationToken) => 
                await blogDbContext.Users.FindAsync(userId) != null) 
            .WithMessage("User not found").WithErrorCode("404");
        RuleFor(x => x.User.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.User.Email)).WithMessage("Email is null")
            .MustAsync(async (email, cancellation) =>
                !await _blogDbContext.Users.AnyAsync(u => u.Email == email, cancellation))
            .WithMessage("Email already exists");
        RuleFor(x=>x).Must(x=>!string.IsNullOrWhiteSpace(x.User.Email)||
                              !string.IsNullOrWhiteSpace(x.User.Name)|| !string.IsNullOrWhiteSpace(x.User.Password))
            .WithMessage("At least one field is required. ");
    }
}