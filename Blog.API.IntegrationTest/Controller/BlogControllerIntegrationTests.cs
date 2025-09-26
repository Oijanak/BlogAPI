using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;
using BlogApi.Domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.IntegrationTest.Controller;

public class BlogControllerIntegrationTests:IClassFixture<BlogApiWebFactory>
{
    private readonly BlogApiWebFactory _factory;
    private readonly HttpClient _client;

    public BlogControllerIntegrationTests(BlogApiWebFactory factory)
    {
        _factory=factory;
        _client=factory.CreateClient();
        var user = new 
        {
            Name = "user",
            Email = "user02@example.com",
            Password = "User123!"
        };

        _client.PostAsJsonAsync("/api/Auth/register", user).GetAwaiter().GetResult();
     
    }
    
    [Fact]
    public async Task CreateBlog_ShouldReturn_Created()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        
        var author = await CreateTestAuthorAsync("Author","test001@example.com");
        var blog = new CreateBlogCommand
        {
            BlogTitle = "Test Blog",
            BlogContent = "This is a test blog.",
            AuthorId = author.AuthorId,
            StartDate = new DateTime(2025,09,25),
            EndDate = new DateTime(2025,09,27)
        };

        var response = await _client.PostAsJsonAsync("/api/blogs", blog);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        createdBlog.Should().NotBeNull();
        createdBlog.Data.BlogTitle.Should().Be("Test Blog");
        createdBlog.Data.ActiveStatus.Should().Be(ActiveStatus.Active);
        createdBlog.Data.ApproveStatus.Should().Be(ApproveStatus.Pending);
        createdBlog.Data.BlogContent.Should().Be("This is a test blog.");
    }
    
    [Fact]
    public async Task CreateBlog_ForInvalidAuthorId_ShouldReturn_NotFound()
    {

        var blog = new CreateBlogCommand
        {
            BlogTitle = "Invalid Blog",
            BlogContent = "This blog has an invalid author.",
            AuthorId = Guid.NewGuid() 
        };
        
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        

        var response = await _client.PostAsJsonAsync("/api/blogs", blog);
      
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true ,Converters = { new JsonStringEnumConverter() } });
       error.Should().NotBeNull();
       error.Errors["AuthorId"].Should().Contain("Author not found");
       error.Status.Should().Be(400);

    }
    
    
    [Fact]
    public async Task DeleteBlog_ShouldReturn_Ok()
    {
        var author = await CreateTestAuthorAsync("Janak","janak50@gmail.com");
        
        var createBlog = new CreateBlogCommand
        {
            AuthorId = author.AuthorId,
            BlogTitle = "Delete Blog",
            BlogContent = "Content to delete",
            StartDate = new DateTime(2025,09,25),
            EndDate = new DateTime(2025,09,27)
        };
        var createResponse = await _client.PostAsJsonAsync("/api/blogs", createBlog);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        var response = await _client.DeleteAsync($"/api/blogs/{createdBlog.Data.BlogId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleteResult = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,  });

        deleteResult.Message.Should().Be("Blog deleted successfully");
    }
    
    [Fact]
    public async Task GetBlogById_ShouldReturn_Ok_WhenExists()
    {
        var author = await CreateTestAuthorAsync("GetAuthor","get01@gmail.com");
        var blog = new CreateBlogCommand
        {
            AuthorId = author.AuthorId,
            BlogTitle = "New Blog",
            BlogContent = "Blog content",
            StartDate = new DateTime(2025,09,25),
            EndDate = new DateTime(2025,09,27)
        };

        var createResponse = await _client.PostAsJsonAsync("/api/blogs", blog);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        var blogId = createdBlog.Data.BlogId;

        var response = await _client.GetAsync($"/api/blogs/{blogId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });
        fetchedBlog.Data.BlogId.Should().Be(blogId);
    }
    
    [Fact]
    public async Task GetAllBlogs_ShouldReturn_Ok()
    {
        var response = await _client.PostAsJsonAsync("/api/blogs/getAll",new {});
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var blogs = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<BlogDTO>>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        blogs.Should().NotBeNull();
        blogs.Data.Should().NotBeNull();
    }   
    
    
    private async Task<AuthorDto> CreateTestAuthorAsync(string name = "Test Author", string email = "test1@gmail.com", int age = 35)
    {
        var author = new CreateAuthorCommand
        { 
            AuthorName= name,
            AuthorEmail = email,
            Age = age
        };
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJsonAsync("/api/authors", author);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdAuthor = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return createdAuthor.Data;
    }
    
    private async Task<string> GetJwtTokenAsync()
    {
        var loginRequest = new
        {
            Email = "user02@example.com",
            Password = "User123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        var json = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return json.AccessToken;
    }
    
    
}
public class ValidationErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}

