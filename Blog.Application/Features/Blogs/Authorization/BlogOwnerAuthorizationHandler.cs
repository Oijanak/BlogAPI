using BlogApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BlogApi.Application.Features.Blogs.Authorization;

public class BlogOwnerAuthorizationHandler:AuthorizationHandler<BlogOwnerAuthorizationRequirement>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlogDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BlogOwnerAuthorizationHandler(ICurrentUserService currentUserService, IBlogDbContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlogOwnerAuthorizationRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var routeData = httpContext.GetRouteData();
        var blogId = Guid.Parse(routeData.Values["BlogId"]!.ToString()!);
        var blog= await _dbContext.Blogs.FindAsync(blogId);
        if (blog == null)
        {
            context.Succeed(requirement);
        }
        if (blog != null && blog.CreatedBy == _currentUserService.UserId)
        {
            context.Succeed(requirement);
        }
    }
}