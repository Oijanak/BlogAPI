using BlogApi.Application.Interfaces;
using BlogApi.Infrastructure.Data;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.API.IntegrationTest;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class BlogApiWebFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == 
                     typeof(IDbContextOptionsConfiguration<BlogDbContext>));
            if(dbContextDescriptor!=null)
                services.Remove(dbContextDescriptor);
            
            services.AddDbContext<IBlogDbContext,BlogDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
            
        });
    }
}