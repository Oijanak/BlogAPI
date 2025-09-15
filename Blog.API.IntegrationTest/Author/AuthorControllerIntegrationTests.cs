using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using FluentAssertions;

namespace Blog.API.IntegrationTest.Author;

public class AuthorControllerIntegrationTests:IClassFixture<BlogApiWebFactory<Program>>
{
    private readonly BlogApiWebFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthorControllerIntegrationTests(BlogApiWebFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    [Fact]
    public async Task CreateAuthor_ShouldReturn_Created()
    {
        var created = await CreateTestAuthorAsync("New Author","author@gmail.com",35);
        created.Should().NotBeNull();
        created.AuthorId.Should().NotBeEmpty();
        created.AuthorName.Should().Be("New Author");
        created.Age.Should().Be(35);
        created.AuthorEmail.Should().Be("author@gmail.com");
    }
    
    
    [Fact]
    public async Task UpdateUser_ShouldReturn_Ok_WhenUserExists()
    {
        var createdUser = await CreateTestAuthorAsync("Janak","janak@gmail.com");

        var updateCommand = new AuthorRequest
        {
                AuthorName = "Updated Name",
                AuthorEmail = "updated@example.com",
                Age = 20
        };

        var response = await _client.PutAsJsonAsync($"/api/authors/{createdUser.AuthorId}", updateCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedUser = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        updatedUser.Data.AuthorName.Should().Be("Updated Name");
        updatedUser.Data.AuthorEmail.Should().Be("updated@example.com");
        updatedUser.Data.Age.Should().Be(20);
    }
    
    [Fact]
    public async Task GetAuthors_ShouldReturn_Ok()
    {
        var response = await _client.GetAsync("/api/authors");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var authors = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<AuthorDto>>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        authors.Should().NotBeNull();
    }
    
    [Fact]
    public async Task DeleteAuthor_ShouldReturn_Ok()
    {
        var createdAuthor = await CreateTestAuthorAsync();

        var response = await _client.DeleteAsync($"/api/authors/{createdAuthor.AuthorId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        result.Message.Should().Be("Author deleted successfully");
    }

    [Fact]
    public async Task GetAuthorById_Should_Return_Ok()
    {
        var createdAuthor = await CreateTestAuthorAsync("GetAuthor","getauthor@gmail.com");
        var response = await _client.GetAsync($"/api/authors/{createdAuthor.AuthorId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(new JsonSerializerOptions
            { PropertyNameCaseInsensitive = true });
        result.Data.Should().NotBeNull();
        result.Data.AuthorId.Should().Be(createdAuthor.AuthorId);
        result.Data.AuthorName.Should().Be(createdAuthor.AuthorName);
        result.Data.AuthorEmail.Should().Be(createdAuthor.AuthorEmail);
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
    

    
}