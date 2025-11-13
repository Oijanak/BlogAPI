using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlogApi.Application.Options;
using BlogApi.Application.Dapper.Blogs.Commands.CreateBlogWithDapperCommand;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;
using BlogApi.Domain.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Blog.API.IntegrationTest.Controller;

public class BlogControllerIntegrationTests:IClassFixture<BlogApiWebFactory>,IDisposable
{
    private readonly BlogApiWebFactory _factory;
    private readonly HttpClient _client;
    private readonly string _uploadPath;

    public BlogControllerIntegrationTests(BlogApiWebFactory factory)
    {
        _factory=factory;
        _client=factory.CreateClient();
        using var scope = factory.Services.CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var option = scope.ServiceProvider.GetRequiredService<IOptions<FileStorageOptions>>();

        _uploadPath = Path.Combine(env.ContentRootPath, option.Value.UploadFolder);
        
        var user = new 
        {
            Name = "user",
            Email = "user02@example.com",
            Password = "User123!",
            Role = "Maker"
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
        var form = new MultipartFormDataContent();
        
        form.Add(new StringContent("Test Blog"), nameof(CreateBlogCommand.BlogTitle));
        form.Add(new StringContent("This is a test blog."), nameof(CreateBlogCommand.BlogContent));
        form.Add(new StringContent(author.AuthorId.ToString()), nameof(CreateBlogCommand.AuthorId));
        form.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
        form.Add(new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogCommand.EndDate));
        var file1 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is document 1 content."));
        file1.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(file1, "Documents", "doc1.txt");

        var file2 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is document 2 content."));
        file2.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(file2, "Documents", "doc2.txt");
        
        var response = await _client.PostAsync("/api/blogs", form);
        var responseBody = await response.Content.ReadAsStringAsync();
        var createdBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        createdBlog.Should().NotBeNull();
        createdBlog.Data.BlogTitle.Should().Be("Test Blog");
        createdBlog.Data.ActiveStatus.Should().Be(ActiveStatus.Active);
        createdBlog.Data.ApproveStatus.Should().Be(ApproveStatus.Pending);
        createdBlog.Data.BlogContent.Should().Be("This is a test blog.");
        createdBlog.Data.BlogDocuments.Count.Should().Be(2);    
        
    }
    
    [Fact]
    public async Task CreateBlog_ForInvalidAuthorId_ShouldReturn_NotFound()
    {
        var form = new MultipartFormDataContent();
        form.Add(new StringContent("Test Blog"), nameof(CreateBlogCommand.BlogTitle));
        form.Add(new StringContent("This is a test blog."), nameof(CreateBlogCommand.BlogContent));
        form.Add(new StringContent(Guid.NewGuid().ToString()), nameof(CreateBlogCommand.AuthorId));
        form.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
        form.Add(new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogCommand.EndDate));
        var file1 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is document 1 content."));
        file1.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(file1, "Documents", "doc1.txt");

        var file2 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is document 2 content."));
        file2.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(file2, "Documents", "doc2.txt");
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        

        var response = await _client.PostAsync("/api/blogs", form);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
       error.Should().NotBeNull();
       error.Errors.Should().Contain("Author not found");
       error.StatusCode.Should().Be(404);
    }
    
    
    [Fact]
    public async Task DeleteBlog_ShouldReturn_Ok()
    {
        var author = await CreateTestAuthorAsync("Janak","janak50@gmail.com");
        
        var form = new MultipartFormDataContent();
        
        form.Add(new StringContent("Test Blog"), nameof(CreateBlogCommand.BlogTitle));
        form.Add(new StringContent("This is a test blog."), nameof(CreateBlogCommand.BlogContent));
        form.Add(new StringContent(author.AuthorId.ToString()), nameof(CreateBlogCommand.AuthorId));
        form.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
        form.Add(new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogCommand.EndDate));


        var createResponse = await _client.PostAsync("/api/blogs", form);
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
        var form = new MultipartFormDataContent();
        
        form.Add(new StringContent("Test Blog"), nameof(CreateBlogCommand.BlogTitle));
        form.Add(new StringContent("This is a test blog."), nameof(CreateBlogCommand.BlogContent));
        form.Add(new StringContent(author.AuthorId.ToString()), nameof(CreateBlogCommand.AuthorId));
        form.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
        form.Add(new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogCommand.EndDate));

        var file1 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is document 1 content."));
        file1.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(file1, "Documents", "doc1.txt");

        var file2 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("This is document 2 content."));
        file2.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        form.Add(file2, "Documents", "doc2.txt");

        var createResponse = await _client.PostAsync("/api/blogs", form);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        var blogId = createdBlog.Data.BlogId;

        var response = await _client.GetAsync($"/api/blogs/{blogId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });
        fetchedBlog.Data.BlogId.Should().Be(blogId);
        fetchedBlog.Data.BlogDocuments.Count.Should().Be(2);
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
        var responseBody = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdAuthor = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return createdAuthor.Data;
    }
    
    private async Task<string> GetJwtTokenAsync()
    {
        var loginRequest = new
        {
            Email = "userauth@example.com",
            Password = "User123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        var content =await response.Content.ReadAsStringAsync();
        var json = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>();
        
        return json.Data.AccessToken;
    }
    
     [Fact]
    public async Task UpdateBlog_ShouldReturn_Ok_WithUpdatedData()
    {
      
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var author = await CreateTestAuthorAsync("AuthorToUpdate", "author_update@example.com");

        
        var createForm = new MultipartFormDataContent();
        createForm.Add(new StringContent("Original Blog"), nameof(CreateBlogCommand.BlogTitle));
        createForm.Add(new StringContent("Original content"), nameof(CreateBlogCommand.BlogContent));
        createForm.Add(new StringContent(author.AuthorId.ToString()), nameof(CreateBlogCommand.AuthorId));
        createForm.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
        createForm.Add(new StringContent(DateTime.UtcNow.AddDays(5).ToString("o")), nameof(CreateBlogCommand.EndDate));

        var createResponse = await _client.PostAsync("/api/blogs", createForm);
        var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } });
        
        var updateForm = new MultipartFormDataContent();
        updateForm.Add(new StringContent(author.AuthorId.ToString()), "Blog.AuthorId");
        updateForm.Add(new StringContent("Updated Blog Title"), "Blog.BlogTitle");
        updateForm.Add(new StringContent("Updated content"), "Blog.BlogContent");
        updateForm.Add(new StringContent(DateTime.UtcNow.ToString("o")), "Blog.StartDate");
        updateForm.Add(new StringContent(DateTime.UtcNow.AddDays(10).ToString("o")), "Blog.EndDate");
        
        var file1 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Updated doc1"));
        file1.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        updateForm.Add(file1, "Blog.Documents", "updated1.txt");

        var file2 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Updated doc2"));
        file2.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        updateForm.Add(file2, "Blog.Documents", "updated2.txt");
        
        var response = await _client.PutAsync($"/api/blogs/{createdBlog.Data.BlogId}", updateForm);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } });

        updatedBlog.Data.BlogTitle.Should().Be("Updated Blog Title");
        updatedBlog.Data.BlogContent.Should().Be("Updated content");
        updatedBlog.Data.BlogDocuments.Count.Should().Be(2);
    }
    
    [Fact]
    public async Task UpdateBlog_AsUnauthorizedUser_ShouldReturn_Forbidden()
    {
    var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", new { Email = "blogger@example.com", Password =  "Blogger123!"});
    var content = await loginResponse.Content.ReadAsStringAsync();
    var loginResult = await loginResponse.Content.ReadFromJsonAsync<Result<TokenResponse>>();
    var token = loginResult.Data.AccessToken;

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    
    var author = await CreateTestAuthorAsync("Original Author", "author01@example.com");
    var createForm = new MultipartFormDataContent();
    createForm.Add(new StringContent("Original Blog"), nameof(CreateBlogCommand.BlogTitle));
    createForm.Add(new StringContent("Original Content"), nameof(CreateBlogCommand.BlogContent));
    createForm.Add(new StringContent(author.AuthorId.ToString()), nameof(CreateBlogCommand.AuthorId));
    createForm.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
    createForm.Add(new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogCommand.EndDate));

    var createResponse = await _client.PostAsync("/api/blogs", createForm);
    var createdBlog = await createResponse.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } });
    
    var updateForm = new MultipartFormDataContent();
    updateForm.Add(new StringContent(author.AuthorId.ToString()), "Blog.AuthorId");
    updateForm.Add(new StringContent("Updated Title"), "Blog.BlogTitle");
    updateForm.Add(new StringContent("Updated Content"), "Blog.BlogContent");
    updateForm.Add(new StringContent(DateTime.UtcNow.ToString("o")), "Blog.StartDate");
    updateForm.Add(new StringContent(DateTime.UtcNow.AddDays(5).ToString("o")), "Blog.EndDate");

    var newToken=await GetJwtTokenAsync();
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
   
    var response = await _client.PutAsync($"/api/blogs/{createdBlog.Data.BlogId}", updateForm);
    
    response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        
   
    
    }
    
    [Fact]
    public async Task CreateBlogWithDapper_ShouldReturn_Created()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var author = await CreateTestAuthorAsync("Author", "author_dapper@example.com");

        var form = new MultipartFormDataContent
        {
            { new StringContent(author.AuthorId.ToString()), nameof(CreateBlogWithDapperCommand.AuthorId) },
            { new StringContent("Test Dapper Blog"), nameof(CreateBlogWithDapperCommand.BlogTitle) },
            { new StringContent("This is a test blog created with Dapper"), nameof(CreateBlogWithDapperCommand.BlogContent) },
            { new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogWithDapperCommand.StartDate) },
            { new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogWithDapperCommand.EndDate) }
        };
        
        var file1 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Doc 1 content"));
        file1.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        form.Add(file1, "Files", "doc1.txt");

        var file2 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes("Doc 2 content"));
        file2.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        form.Add(file2, "Files", "doc2.txt");

        var response = await _client.PostAsync("/api/blogs/dapper", form);
        var responseBody = await response.Content.ReadAsStringAsync();
        
        var responsebody = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdBlog = await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } });

        createdBlog.Data.BlogTitle.Should().Be("Test Dapper Blog");
        createdBlog.Data.BlogDocuments.Count.Should().Be(2);
    }



    public void Dispose()
    {
        if (Directory.Exists(_uploadPath))
        {
            Directory.Delete(_uploadPath, recursive: true);
        }
    }
    
}
public class ValidationErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}

