using System.Data;
using System.Text;
using Ardalis.GuardClauses;
using Blog.API.Filters;
using BlogApi.API.Controllers.Middlewares;
using BlogApi.Application;
using BlogApi.Application.Interfaces;
using BlogApi.Application.DTOs;
using BlogApi.Application.DTOs.Validators;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File
    (
        path: "Logs/log.txt",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Host.UseSerilog();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Guard.Against.NullOrEmpty(connectionString,nameof(connectionString));
builder.Services.AddDbContext<IBlogDbContext,BlogDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BlogDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAuthorCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddSwaggerGen(options=>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
    options.OperationFilter<AuthOperationFilter>();

    
});
builder.Services.AddControllers(
        options=>options.Filters.Add<RequestResponseLoggingFilter>())
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddApplication();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITokenService, JwtTokenService>();

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];
Guard.Against.NullOrEmpty(jwtIssuer, nameof(jwtIssuer));
Guard.Against.NullOrEmpty(jwtAudience, nameof(jwtAudience));
Guard.Against.NullOrEmpty(jwtKey, nameof(jwtKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(jwtKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer =jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(
            new ApiErrorResponse{ StatusCode = 404, Message = $"Resource not found {context.HttpContext.Request.Method} {context.HttpContext.Request.Path}" }
        );
    }
});
app.UseMiddleware<GlobalExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program
{
    
}
