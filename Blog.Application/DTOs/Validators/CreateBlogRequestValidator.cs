using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.DTOs.Validators;

public class CreateBlogRequestValidator:AbstractValidator<CreateBlogRequest>
{
    private  readonly IBlogDbContext _blogDbContext; 
    public CreateBlogRequestValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.BlogTitle).NotEmpty().WithMessage("Blog title is required")
            .MaximumLength(200).WithMessage("Blog title cannot exceed 200 characters");
        RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog content is required");
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
    }
    
}