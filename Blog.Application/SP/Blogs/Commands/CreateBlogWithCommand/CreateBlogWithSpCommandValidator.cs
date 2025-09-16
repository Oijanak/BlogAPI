using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.SP.Blogs.Commands;

public class CreateBlogWithSpCommandValidator:AbstractValidator<CreateBlogWithSpCommand>
{
    private readonly IBlogDbContext _blogDbContext;
    public CreateBlogWithSpCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is Empty")
            .NotNull().WithMessage("AuthorId is Null")
            .MustAsync(async (authorId, cancellationToken) => 
                await blogDbContext.Authors.FindAsync(authorId) != null).WithErrorCode("404")
            .WithMessage("Author not found");
        RuleFor(x => x.BlogTitle).NotEmpty().WithMessage("Blog Title is Required").MaximumLength(200).WithMessage("Blog Title Maximum length is 200 characters");
        RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog Content is Required");
    }
    
}