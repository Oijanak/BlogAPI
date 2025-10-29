using System.Data;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Models;
using BlogApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blog.API.IntegrationTest;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class BlogApiWebFactory
    : WebApplicationFactory<Program> 
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config
                .AddJsonFile("appsettings.Test.json", optional: false);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<BlogDbContext>));
            
            var sp = services.BuildServiceProvider();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var testConnection = configuration.GetConnectionString("TestConnection");
            
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(testConnection);
            });
            services.RemoveAll<IDbConnection>();
            services.AddScoped<IDbConnection>(sp =>
            {
                return new SqlConnection(testConnection);
            
            });
            
            using var scope = services.BuildServiceProvider().CreateScope();
            var scopedProvider = scope.ServiceProvider;
            var dbContext = scopedProvider.GetRequiredService<BlogDbContext>();
            var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
            dbContext.Database.EnsureDeleted(); 
            dbContext.Database.Migrate();
            var user = new User()
            {
                Email = "userauth@example.com",
                UserName = "userauth@example.com",
                EmailConfirmed = true
            };
            roleManager.CreateAsync(new IdentityRole("Maker")).GetAwaiter().GetResult();
            userManager.CreateAsync(user, "User123!").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, "Maker").GetAwaiter().GetResult();
            var newUser = new User()
            {
                UserName = "blogger@example.com",
                Email = "blogger@example.com",
                EmailConfirmed = true
            };
            userManager.CreateAsync(newUser, "Blogger123!").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(newUser, "Maker").GetAwaiter().GetResult();
            
        });
        
    }
}