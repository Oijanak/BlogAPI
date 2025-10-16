using System.Data;
using BlogApi.Application.Interfaces;
using BlogApi.Infrastructure.Data;
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

            
            dbContext.Database.EnsureDeleted(); 
            dbContext.Database.Migrate();
            
        });
        
    }
}