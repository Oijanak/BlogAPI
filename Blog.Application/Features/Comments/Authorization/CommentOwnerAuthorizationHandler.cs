using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BlogApi.Application.Features.Comments.Authorization;

public class CommentOwnerAuthorizationHandler:AuthorizationHandler<CommentOwnerRequirement>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlogDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentOwnerAuthorizationHandler(ICurrentUserService currentUserService,
        IBlogDbContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }


    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, CommentOwnerRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var routeData = httpContext.GetRouteData();
        var commentId = Guid.Parse(routeData.Values["CommentId"]!.ToString()!);
        
        var comment = await _dbContext.Comments.FindAsync(commentId);
        if (comment == null)
        {
            context.Succeed(requirement);
        }
        
        if (comment != null && comment.UserId == _currentUserService.UserId)
        {
            context.Succeed(requirement);
        }
    }
}