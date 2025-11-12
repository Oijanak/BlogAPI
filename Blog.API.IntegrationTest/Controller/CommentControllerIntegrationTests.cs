using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using BlogApi.Application.Features.Blogs.Commands.CreateBlogCommand;
using BlogApi.Application.Features.Comments.CreateCommentCommand;
using BlogApi.Application.Features.Comments.UpdateCommentCommand;
using FluentAssertions;

namespace Blog.API.IntegrationTest.Controller;

public class CommentControllerIntegrationTests:IClassFixture<BlogApiWebFactory>
{
    private readonly BlogApiWebFactory _factory;
    private readonly HttpClient _client;

    public CommentControllerIntegrationTests(BlogApiWebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        var token =  GetJwtTokenAsync().GetAwaiter().GetResult();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
      
    }

    [Fact]
    public async Task CreateComment_ShouldReturn_Created()
    {
        var blog = await CreateTestBlogAsync();
        var command = new CreateCommentCommand()
        {
            BlogId = blog.BlogId,
            Content = "This is a test comment."
        };
        var response = await _client.PostAsJsonAsync("/api/comments", command);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var x = 10;
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<CommentDtos>>();
        result.Should().NotBeNull();
        result!.Message.Should().Be("Comment created successfully");
        result.Data.Should().NotBeNull();
        result.Data.Content.Should().Be(command.Content);
        result.Data.CommentId.Should().NotBeEmpty();
    }
    
    
    [Fact]
    public async Task UpdateComment_ShouldReturn_Ok()
    {
        var blog = await CreateTestBlogAsync();
        var command = new CreateCommentCommand()
        {
            BlogId = blog.BlogId,
            Content = "This is a test comment."
        };
        var createResponse = await _client.PostAsJsonAsync("/api/comments", command);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdComment = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CommentDtos>>();
       
        var updateResponse=await _client.PutAsJsonAsync(
            $"/api/comments/{createdComment.Data.CommentId}",  
           new {Content ="Updated Comment"}
        );
        
        var updateBody = await updateResponse.Content.ReadAsStringAsync();
        
        var updatedComment = await updateResponse.Content.ReadFromJsonAsync<ApiResponse<CommentDtos>>();
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedComment.Should().NotBeNull();
        updatedComment.Data.Content.Should().Be("Updated Comment");
        updatedComment.Data.CommentId.Should().Be(createdComment.Data.CommentId);
        
    }
    
    
    [Theory]
    [InlineData(null, "Valid content")] 
    [InlineData("00000000-0000-0000-0000-000000000000", "Valid content")] 
    [InlineData("00000000-0000-0000-0000-000000000000", "")] 
    [InlineData("00000000-0000-0000-0000-000000000000", null)] 
    public async Task CreateComment_WithInvalidRequest_ShouldReturn_BadRequest(string? blogId, string? content)
    {
        var command = new CreateCommentCommand
        {
            BlogId = string.IsNullOrEmpty(blogId) ? Guid.Empty : Guid.Parse(blogId),
            Content = content
        };
        var response = await _client.PostAsJsonAsync("/api/comments", command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responsebody=await response.Content.ReadAsStringAsync();
        
        responsebody.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteComment_ShouldReturn_Ok()
    {
        
        var blog = await CreateTestBlogAsync();
        var command = new CreateCommentCommand()
        {
            BlogId = blog.BlogId,
            Content = "This is a test comment."
        };
        var response = await _client.PostAsJsonAsync("/api/comments", command);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<CommentDtos>>();
        var createdComment = result.Data;
        var deleteResponse = await _client.DeleteAsync($"/api/comments/{createdComment.CommentId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteComment_WithInvalidCommentId_ShouldReturn_NotFound()
    {
        var deleteResponse = await _client.DeleteAsync($"/api/comments/{Guid.NewGuid()}");
        var response = await deleteResponse.Content.ReadAsStringAsync();
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    private async Task<AuthorDto> CreateTestAuthorAsync(string name = "Test Author", string email = "test_author_comment@gmail.com", int age = 35)
    {
        var author = new CreateAuthorCommand
        { 
            AuthorName= name,
            AuthorEmail = email,
            Age = age
        };
        var response = await _client.PostAsJsonAsync("/api/authors", author);
        var responseBody = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdAuthor = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

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

        var json = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>();
        return json.Data.AccessToken;
    }
    
    private async Task<BlogDTO> CreateTestBlogAsync()
    {
        var author = await CreateTestAuthorAsync("Test Author", $"test_author_{new Random().Next()}@gmail.com");
        var form = new MultipartFormDataContent();
        form.Add(new StringContent("Test Blog"), nameof(CreateBlogCommand.BlogTitle));
        form.Add(new StringContent("This is a test blog."), nameof(CreateBlogCommand.BlogContent));
        form.Add(new StringContent(author.AuthorId.ToString()), nameof(CreateBlogCommand.AuthorId));
        form.Add(new StringContent(DateTime.UtcNow.ToString("o")), nameof(CreateBlogCommand.StartDate));
        form.Add(new StringContent(DateTime.UtcNow.AddDays(2).ToString("o")), nameof(CreateBlogCommand.EndDate));
        var response = await _client.PostAsync("/api/blogs", form);
        var code = response.StatusCode;
        var responseBody = await response.Content.ReadAsStringAsync();
        var result= await response.Content.ReadFromJsonAsync<ApiResponse<BlogDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true,Converters = { new JsonStringEnumConverter() } });

        return result.Data;
    }
    
}