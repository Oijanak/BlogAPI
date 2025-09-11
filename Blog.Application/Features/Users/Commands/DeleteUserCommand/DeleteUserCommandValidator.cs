using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using FluentValidation;

namespace BlogApi.Application.Features.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandValidator:AbstractValidator<DeleteUserCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public DeleteUserCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .MustAsync(async (userId, cancellationToken) => 
                await blogDbContext.Users.FindAsync(userId) != null) 
            .WithMessage("User not found").WithErrorCode("404");
        
    }
    
}