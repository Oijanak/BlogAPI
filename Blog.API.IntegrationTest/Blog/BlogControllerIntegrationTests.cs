using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;
using BlogApi.Application.Features.Users.Query.LoginUserRequest;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.IntegrationTest.Blog;

public class BlogControllerIntegrationTests:IClassFixture<BlogApiWebFactory<Program>>
{
    private readonly BlogApiWebFactory<Program> _factory;
    private readonly HttpClient _client;

    public BlogControllerIntegrationTests(BlogApiWebFactory<Program> factory)
    {
        _factory=factory;
        _client=factory.CreateClient();
        var user = new CreateUserCommand
        {
            Name = "user",
            Email = "user@example.com",
            Password = "12345"
        };

        _client.PostAsJsonAsync("/api/users/register", user).GetAwaiter().GetResult();
    }
    

    [Fact]
    public async Task CreateBlog_ShouldReturn_Created_WithJwtToken()
    {
        var author = await CreateTestAuthorAsync("Author","test01@example.com");
        var token = await GetJwtTokenAsync();
        
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var blog = new CreateBlogCommand
        {
            BlogTitle = "Test Blog",
            BlogContent = "This is a test blog.",
            AuthorId = author.AuthorId
        };

        var response = await _client.PostAsJsonAsync("/api/blogs", blog);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        createdBlog.Should().NotBeNull();
        createdBlog.Data.BlogTitle.Should().Be("Test Blog");
        createdBlog.Data.BlogContent.Should().Be("This is a test blog.");
    }
    
    [Fact]
    public async Task CreateBlog_ShouldReturn_NotFound_WhenAuthorDoesNotExist()
    {
        var token = await GetJwtTokenAsync();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var blog = new CreateBlogCommand
        {
            BlogTitle = "Invalid Blog",
            BlogContent = "This blog has an invalid author.",
            AuthorId = Guid.NewGuid() 
        };
        

        var response = await _client.PostAsJsonAsync("/api/blogs", blog);
      
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
       error.Should().NotBeNull();
       error.Errors["AuthorId"].Should().Contain("Author not found");
       error.Status.Should().Be(400);

    }

    
    [Fact]
    public async Task UpdateBlogWithSp_ShouldReturn_Ok()
    {
        var author = await CreateTestAuthorAsync();
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var blog = new CreateBlogCommand
        {
            BlogTitle = "Test Blog",
            BlogContent = "This is a test blog.",
            AuthorId = author.AuthorId
        };
        var createResponse = await _client.PostAsJsonAsync("/api/blogs", blog);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var updateBlog = new UpdateBlogRequest
        {
            AuthorId = author.AuthorId,
            BlogTitle = "SP Blog Updated",
            BlogContent= "Updated content"
        };

        var response = await _client.PatchAsJsonAsync($"/api/blogs/{createdBlog.Data.BlogId}", updateBlog);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        updatedBlog.Data.Should().NotBeNull();
        updatedBlog.Data.BlogContent.Should().Be("Updated content");
        updatedBlog.Data.BlogTitle.Should().Be("SP Blog Updated");
    }
    
    [Fact]
    public async Task UpdateBlogWithSp_ShouldReturn_NotFound_WhenBlogDoesNotExist()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var updateBlog = new UpdateBlogRequest
        {
            AuthorId = Guid.NewGuid(),
            BlogTitle = "SP Blog Updated",
            BlogContent = "Updated content"
        };
        var response = await _client.PatchAsJsonAsync($"/api/blogs/{Guid.NewGuid()}", updateBlog);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }

    
    
    [Fact]
    public async Task DeleteBlog_ShouldReturn_Ok_WithJwtToken()
    {
        var token = await GetJwtTokenAsync();
        var author = await CreateTestAuthorAsync("Janak","janak@gmail.com");
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var createBlog = new CreateBlogCommand
        {
            AuthorId = author.AuthorId,
            BlogTitle = "Delete Blog",
            BlogContent = "Content to delete"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/blogs", createBlog);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var response = await _client.DeleteAsync($"/api/blogs/{createdBlog.Data.BlogId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleteResult = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        deleteResult.Message.Should().Be("Blog deleted successfully");
    }
    
    [Fact]
    public async Task GetBlogById_ShouldReturn_Ok_WhenExists()
    {
        var author = await CreateTestAuthorAsync("GetAuthor","get@gmail.com");
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var blog = new CreateBlogCommand
        {
            AuthorId = author.AuthorId,
            BlogTitle = "New Blog",
            BlogContent = "Blog content"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/blogs", blog);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var blogId = createdBlog.Data.BlogId;

        var response = await _client.GetAsync($"/api/blogs/{blogId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        fetchedBlog.Data.BlogId.Should().Be(blogId);
    }
    
    [Fact]
    public async Task GetAllBlogs_ShouldReturn_Ok()
    {
        var response = await _client.GetAsync("/api/blogs");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var blogs = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<BlogDTO>>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        blogs.Should().NotBeNull();
        blogs.Data.Should().NotBeNull();
    }   
    
    
    private async Task<AuthorDto> CreateTestAuthorAsync(string name = "Test Author", string email = "test@gmail.com", int age = 35)
    {
        var author = new CreateAuthorCommand
        { 
            AuthorName= name,
            AuthorEmail = email,
            Age = age
        };
        var response = await _client.PostAsJsonAsync("/api/authors", author);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdAuthor = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return createdAuthor.Data;
    }
    
    
    
    private async Task<string> GetJwtTokenAsync()
    {
        var loginRequest = new LoginUserRequest
        {
            Email = "user@example.com",
            Password = "12345",
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/users/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        loginResult.Should().NotBeNull();
        return loginResult.Token;
    }
    
}
public class ValidationErrorResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}

