using System.Reflection;
using BlogApi.Application.Common.Validations;
using BlogApi.Application.DTOs;
using BlogApi.Application.DTOs.Validators;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


namespace BlogApi.Application;

public static class ServiceRegistration
{
     public static IServiceCollection AddApplication(this IServiceCollection services)
            {
                services.AddMediatR( cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                services.AddValidatorsFromAssemblyContaining<CreateAuthorCommandValidator>();
                services.AddFluentValidationAutoValidation();
        return services;
            }
    
}