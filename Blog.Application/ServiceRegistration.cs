using System.Reflection;
using BlogApi.Application.Common.Validations;
using BlogApi.Application.Features.Users.Query.LoginUserRequest;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApi.Application;

public static class ServiceRegistration
{
     public static IServiceCollection AddApplication(this IServiceCollection services)
            {
                services.AddMediatR(Assembly.GetExecutingAssembly());
                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                return services;
            }
    
}