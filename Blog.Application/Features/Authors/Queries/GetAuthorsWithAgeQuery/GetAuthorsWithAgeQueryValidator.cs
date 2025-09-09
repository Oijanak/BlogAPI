using FluentValidation;

namespace BlogApi.Application.Features.Authors.Queries.GetAuthorsWithAgeQuery;

public class GetAuthorsWithAgeQueryValidator:AbstractValidator<GetAuthorsWithAgeQuery>
{
    public GetAuthorsWithAgeQueryValidator()
    {
        RuleFor(x=>x.Age1).GreaterThan(0).WithMessage("Age 1 must be greater than 0")
            .NotEmpty().WithMessage("Age1 must not be empty")
            .LessThan(x => x.Age2).WithMessage("Age1 must be less than Age2");;
              RuleFor(x=>x.Age2).GreaterThan(0).WithMessage("Age 2 must be greater than 0")
                    .NotEmpty().WithMessage("Age2 must not be empty")
                    .GreaterThan(x => x.Age1).WithMessage("Age2 must be greater than Age1");;
    }
    
}