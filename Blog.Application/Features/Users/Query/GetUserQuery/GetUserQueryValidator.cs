using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Users.Query.GetUserRequest;

public class GetUserQueryValidator:AbstractValidator<GetUserQuery>
{
    private readonly IBlogDbContext _blogDbContext;
    public GetUserQueryValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is Required")
            .MustAsync(async (userId, cancellationToken) => 
                await blogDbContext.Users.FindAsync(userId) != null) 
            .WithMessage("User not found");;

    }
    
}