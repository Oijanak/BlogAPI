using BlogApi.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.Features.Authors.Commands.UpdateAuthorCommand;

public class UpdateAuthorCommandValidator:AbstractValidator<UpdateAuthorCommand>
{
    private  readonly BlogDbContext _blogDbContext; 
    public UpdateAuthorCommandValidator(BlogDbContext blogDbContext)
    {
        this._blogDbContext = blogDbContext;
        
        RuleFor(x=>x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
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