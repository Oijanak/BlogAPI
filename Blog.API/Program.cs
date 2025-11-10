using Ardalis.GuardClauses;
using Blog.API.Filters;
using BlogApi.API.Controllers.Middlewares;
using BlogApi.Application;
using BlogApi.Application.DTOs;
using BlogApi.Application.DTOs.Validators;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Blogs.Authorization;
using BlogApi.Application.Features.Comments.Authorization;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Services;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using BlogApi.Infrastructure.DbSeeder;
using BlogApi.Infrastructure.Services;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Data;
using System.Text;


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

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true; 
    })
    .AddEntityFrameworkStores<BlogDbContext>()
    .AddDefaultTokenProviders();
   
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(30); 
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAuthorCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});
builder.Services.AddHangfireServer();

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});

builder.Services.AddControllers(
        options=>options.Filters.Add<RequestResponseLoggingFilter>())
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
    
builder.Services.AddApplication();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITokenCleanupService, TokenCleanupService>();
builder.Services.AddScoped<IUpdateBlogActiveStatusService, UpdateBlogActiveStatusService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IExcelService,ExcelService>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection("FileStorage"));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthorizationHandler, CommentOwnerAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, BlogOwnerAuthorizationHandler>();
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];
Guard.Against.NullOrEmpty(jwtIssuer, nameof(jwtIssuer));
Guard.Against.NullOrEmpty(jwtAudience, nameof(jwtAudience));
Guard.Against.NullOrEmpty(jwtKey, nameof(jwtKey));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BlogInstance";
   
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(jwtKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CommentOwnerPolicy", policy =>
        policy.Requirements.Add(new CommentOwnerRequirement()));
    
    options.AddPolicy("BlogOwnerPolicy", policy =>
        policy.Requirements.Add(new BlogOwnerAuthorizationRequirement()));
});;
QuestPDF.Settings.License = LicenseType.Community;
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedRolesAndAdminAsync(services);
    var context = scope.ServiceProvider.GetRequiredService<IBlogDbContext>();
    await FakeBlogSeeder.SeedAsync(context);
}
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
app.UseCors("AllowVue");

var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Blogs");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads","Blogs")),
    RequestPath = "/blogs/files"
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<ITokenCleanupService>(
    "cleanup-expired-refresh-tokens",
    service => service.RemoveExpiredTokensAsync(),
    Cron.Daily
);
RecurringJob.AddOrUpdate<IUpdateBlogActiveStatusService>(
    "update-blog-active-status",
    service => service.UpdateBlogActiveStatusAsync(),
    Cron.Daily
);
app.Run();

public partial class Program
{
    
}
