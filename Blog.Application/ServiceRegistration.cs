using System.Reflection;
using BlogApi.Application.DTOs;
using BlogApi.Application.DTOs.Validators;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace BlogApi.Application;

public static class ServiceRegistration
{
     public static IServiceCollection AddApplication(this IServiceCollection services)
            {
                services.AddMediatR(Assembly.GetExecutingAssembly());
                return services;
            }
    
}