using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.SP.Users.Commands.DeleteUserWithSpCommand;

public class DeleteUserWithSpCommandValidator:AbstractValidator<DeleteUserWithSpCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public DeleteUserWithSpCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required")
            .MustAsync(async (userId, cancellationToken) => 
                await blogDbContext.Users.FindAsync(userId) != null) 
            .WithMessage("User not found").WithErrorCode("404");
        
    }
    
}