using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;

public class CreateBlogCommandValidator: AbstractValidator<CreateBlogCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public CreateBlogCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is required")
            .MustAsync(async (authorId, cancellationToken) =>
                await blogDbContext.Authors.FindAsync(authorId) != null).WithErrorCode("404")
            .WithMessage("Author not found");
        RuleFor(x => x.BlogTitle).NotEmpty().WithMessage("Blog Title is Required").MaximumLength(200).WithMessage("Blog Title Maximum length is 200 characters");
        RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog Content is Required");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date is required");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date is required");
        RuleFor(x => x)
            .Must(x => x.StartDate < x.EndDate)
            .WithMessage("Start Date must be earlier than End Date");
        
    }
    
}