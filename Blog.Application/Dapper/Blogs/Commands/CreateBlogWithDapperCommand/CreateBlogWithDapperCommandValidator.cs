using FluentValidation;

namespace BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;

public class CreateBlogWithDapperCommandValidator:AbstractValidator<CreateBlogWithDapperCommand>
{
    public CreateBlogWithDapperCommandValidator()
    {
        RuleFor(x => x.AuthorId).NotEmpty().WithMessage("AuthorId is required");
        RuleFor(x => x.BlogTitle).NotEmpty().WithMessage("Blog Title is Required").MaximumLength(200).WithMessage("Blog Title Maximum length is 200 characters");
        RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog Content is Required");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date is required");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date is required");
        RuleFor(x => x)
            .Must(x => x.StartDate < x.EndDate)
            .WithMessage("Start Date must be earlier than End Date");

    }
    
}