using FluentValidation;

namespace BlogApi.Application.Features.Blogs.Queries.GetBlogQuery;

public class GetBlogQueryValidator:AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(b => b.BlogId).GreaterThan(0).WithMessage("BlogId is invalid");
    }
}