using FluentValidation;

namespace BlogApi.Application.Dapper.Blogs.Commands.UpdateBlogWithDapperCommand;

public class UpdateBlogWithDapperCommandValidator:AbstractValidator<UpdateBlogWithDappersCommand>
{
    public UpdateBlogWithDapperCommandValidator()
    {
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog Id is Required");;
        RuleFor(x => x.Blog.BlogTitle).MaximumLength(200).WithMessage("Blog Title should be of length 200 characters");
        RuleFor(x=>x.Blog.AuthorId).NotEmpty().WithMessage("Author Id is Required");;
        RuleFor(x=>x.Blog.BlogContent).NotEmpty().WithMessage("Blog Content is Required");;
       
    }
}