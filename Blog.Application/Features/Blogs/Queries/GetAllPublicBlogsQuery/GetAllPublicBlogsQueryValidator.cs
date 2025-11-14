using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Features.Blogs.Queries.GetAllPulicBlogsQuery;




    public class GetAllPublicBlogsQueryValidator : AbstractValidator<GetAllPublicBlogsQuery>
    {
        public GetAllPublicBlogsQueryValidator()
        {
         
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0.");

            
            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Limit must be between 1 and 100.");

           
            RuleFor(x => x.Search)
                .MaximumLength(200)
                .WithMessage("Search text cannot exceed 200 characters.");

            
            RuleFor(x => x.SortBy)
                .Must(value => value == null || AllowedSortFields.Contains(value.ToLower()))
                .WithMessage("Invalid SortBy field. Allowed: CreatedAt, BlogTitle, Author");

            
            RuleFor(x => x.SortOrder)
                .Must(value => value == null || value.ToLower() == "asc" || value.ToLower() == "desc")
                .WithMessage("SortOrder must be either 'asc' or 'desc'.");

         
            RuleFor(x => x.Author)
                .MaximumLength(100)
                .WithMessage("Author name is too long.");

            
        }

    private static readonly HashSet<string> AllowedSortFields = new()
    {
        "createdat",
        "blogtitle",
        "author"
    };
}

