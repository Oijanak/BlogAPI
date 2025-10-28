using BlogApi.Application.Features.Blogs.Queries.GetBlogListQuery;
using BlogApi.Domain.Enum;
using FluentValidation;

public class GetBlogListQueryValidator : AbstractValidator<GetBlogListQuery>
{
    public GetBlogListQueryValidator()
    {
        
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 100)
            .WithMessage("Limit must be between 1 and 100.");

        
        RuleFor(x => x.SortOrder)
            .Must(so => string.IsNullOrEmpty(so) || so.Equals("asc", StringComparison.OrdinalIgnoreCase) || so.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortOrder must be 'asc' or 'desc'.");

        var allowedSortBy = new[] { "CreatedAt", "UpdatedAt", "BlogTitle", "StartDate", "EndDate", "Author" };
        RuleFor(x => x.SortBy)
            .Must(sb => string.IsNullOrEmpty(sb) || allowedSortBy.Contains(sb, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"SortBy must be one of: {string.Join(", ", allowedSortBy)}");

        
        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.EndDate >= x.StartDate)
            .WithMessage("EndDate must be greater than or equal to StartDate.");
    }
}
