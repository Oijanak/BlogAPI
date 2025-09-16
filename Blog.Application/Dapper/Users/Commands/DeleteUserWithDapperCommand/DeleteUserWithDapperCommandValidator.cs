using FluentValidation;

namespace BlogApi.Application.Dapper.Users.Commands.DeleteUserWithDapperCommand;

public class DeleteUserWithDapperCommandValidator:AbstractValidator<DeleteUserWithDapperCommand>
{
    public DeleteUserWithDapperCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
    }
    
}