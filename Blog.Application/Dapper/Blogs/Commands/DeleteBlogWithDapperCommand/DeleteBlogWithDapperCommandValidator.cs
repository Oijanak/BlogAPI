using FluentValidation;

namespace BlogApi.Application.Dapper.Blogs.Commands.DeleteBlogWithDapperCommand;

public class DeleteBlogWithDapperCommandValidator:AbstractValidator<DeleteBlogWithDapperCommand>
{
    public DeleteBlogWithDapperCommandValidator()
    {
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("BlogId is Required");
    }
    
}