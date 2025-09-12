namespace Blog.API.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

public class AuthorizeUser : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userIdClaim = context.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "userId");
        Guid.TryParse(userIdClaim.Value, out var claimUserId);
        
        if (context.RouteData.Values.TryGetValue("UserId", out var routeUserIdObj)
            && Guid.TryParse(routeUserIdObj?.ToString(), out var userId))
        {
            if (userId != claimUserId)
            {
                context.Result = new JsonResult(new
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Access denied: User not Authorized"
                })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
        }
    }
}
