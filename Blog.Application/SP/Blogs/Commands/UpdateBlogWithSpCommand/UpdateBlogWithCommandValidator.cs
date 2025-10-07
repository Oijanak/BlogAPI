using BlogApi.Application.Interfaces;
using FluentValidation;

namespace BlogApi.Application.SP.Blogs.Commands.UpdateBlogWithSpCommand;

public class UpdateBlogWithCommandValidator:AbstractValidator<UpdateBlogWithSpCommand>
{
    private readonly IBlogDbContext _blogDbContext;

   public UpdateBlogWithCommandValidator(IBlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog Id is Required")
            .MustAsync(async (blogId, cancellationToken) =>
                await _blogDbContext.Blogs.FindAsync(blogId) != null)
            .WithMessage("Blog not found").WithErrorCode("404");;
        RuleFor(x => x.Blog.BlogTitle).MaximumLength(200).WithMessage("Blog Title should be of length 200 characters");
        RuleFor(x=>x.Blog.AuthorId).NotEmpty().WithMessage("Author Id is Required")
            .MustAsync(async (authorId, cancellationToken) =>
                await _blogDbContext.Authors.FindAsync(authorId) != null)
            .WithMessage("Author not found").WithErrorCode("404");;
        RuleFor(x => x.Blog.StartDate).NotEmpty().WithMessage("Start Date is required");
        RuleFor(x => x.Blog.EndDate).NotEmpty().WithMessage("End Date is required");
        RuleFor(x => x)
            .Must(x => x.Blog.StartDate < x.Blog.EndDate);
    }
    
}