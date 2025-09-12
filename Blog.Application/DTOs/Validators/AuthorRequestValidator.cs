using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Application.DTOs.Validators;

public class AuthorRequestValidator:AbstractValidator<AuthorRequest>
{
    
    private  readonly IBlogDbContext _blogDbContext; 
    public AuthorRequestValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
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