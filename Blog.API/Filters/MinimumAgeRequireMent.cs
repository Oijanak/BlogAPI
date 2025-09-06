using System.Net;
using BlogApi.Application.Exceptions;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.API.Filters;

public class AgeRequirementFilter: IAsyncAuthorizationFilter
{
    private readonly BlogDbContext _blogDbContext;

    public AgeRequirementFilter(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        
          if (context.RouteData.Values.TryGetValue("authorId", out var authorIdValue) &&
                    int.TryParse(authorIdValue?.ToString(), out int authorId))
        {
            Author author=await _blogDbContext.Authors.FindAsync(authorId);
            if (author is not null)
            {
                if (author.Age < 18)
                {
                    throw new ApiException("Author age must be greater than 18.",HttpStatusCode.Forbidden);
                }
            }
        }
        
        await Task.CompletedTask;
    }
}
public class AgeRequirement : TypeFilterAttribute
{
    public AgeRequirement() 
        : base(typeof(AgeRequirementFilter))
    {
        
    }
}
