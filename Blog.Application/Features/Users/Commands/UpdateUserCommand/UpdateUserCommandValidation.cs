using BlogApi.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    private readonly BlogDbContext _blogDbContext;
    public UpdateUserCommandValidation(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.UserId).NotEmpty()
            .WithMessage("UserId is required").MustAsync(async (userId, cancellationToken) => 
                await blogDbContext.Users.FindAsync(userId) != null) 
            .WithMessage("User not found").WithErrorCode("404");
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email is null")
            .MustAsync(async (email, cancellation) =>
                !await _blogDbContext.Users.AnyAsync(u => u.Email == email, cancellation))
            .WithMessage("Email already exists");
        RuleFor(x=>x).Must(x=>!string.IsNullOrWhiteSpace(x.Email)||
                              !string.IsNullOrWhiteSpace(x.Name)|| !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("At least one field is required. ");
    }
    
}