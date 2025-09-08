using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using FluentValidation;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandValidator:AbstractValidator<DeleteUserCommand>
{
    private readonly BlogDbContext _blogDbContext;
    public DeleteUserCommandValidator(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .MustAsync(async (userId, cancellationToken) => 
                await blogDbContext.Users.FindAsync(userId) != null) 
            .WithMessage("User not found").WithErrorCode("404");
        
    }
    
}