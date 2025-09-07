using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryValidator:AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(b => b.BlogId).NotEmpty().WithMessage("BlogId is required");
    }
}