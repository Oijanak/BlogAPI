using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Authors.Commands.CreateAuthorCommand;
using FluentAssertions;

namespace Blog.API.IntegrationTest.Controller;

public class AuthorControllerIntegrationTests:IClassFixture<BlogApiWebFactory>
{
    private readonly BlogApiWebFactory _factory;
    private readonly HttpClient _client;

    public AuthorControllerIntegrationTests(BlogApiWebFactory factory)
    {
      
        _factory = factory;
        _client = _factory.CreateClient();
        var user = new 
        {
            Name = "user",
            Email = "user@example.com",
            Password = "User123!"
        };

        _client.PostAsJsonAsync("/api/Auth/register", user).GetAwaiter().GetResult();
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
    public async Task CreateAuthor_ForInvalidInput_ShouldReturn_BadRequest()
    {
        var author = new CreateAuthorCommand
        {
            AuthorName = "",                
            AuthorEmail = "invalid-email",  
            Age = 35
        };
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/authors", author);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    
    
    [Fact]
    public async Task UpdateUser_WhenUserExists_ShouldReturn_Ok()
    {
        var createdUser = await CreateTestAuthorAsync("Janak","janak@gmail.com");
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var updateCommand = new AuthorRequest
        {
                AuthorName = "Updated Name",
                AuthorEmail = "updated1@example.com",
                Age = 20
        };

        var response = await _client.PutAsJsonAsync($"/api/authors/{createdUser.AuthorId}", updateCommand);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedUser = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        updatedUser.Data.AuthorName.Should().Be("Updated Name");
        updatedUser.Data.AuthorEmail.Should().Be("updated1@example.com");
        updatedUser.Data.Age.Should().Be(20);
    }
    
    [Fact]
    public async Task UpdateAuthor_WhenAuthorDoesNotExist_ShouldReturn_NotFound()
    {
        var nonExistentAuthorId = Guid.NewGuid(); 

        var updateCommand = new AuthorRequest
        {
            AuthorName = "Updated Name",
            AuthorEmail = "updated@example.com",
            Age = 25
        };
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PutAsJsonAsync($"/api/authors/{nonExistentAuthorId}", updateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }
    
    [Theory]
    [InlineData("", "test@example.com", 25)]        
    public async Task UpdateAuthor_ForInvalidUpdateRequest_ShouldReturn_BadRequest(
        string authorName, string authorEmail, int age)
    {
        var createdAuthor = await CreateTestAuthorAsync("Valid Author", "valid@example.com");
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var invalidUpdateCommand = new AuthorRequest
        {
            AuthorName = authorName,
            AuthorEmail = authorEmail,
            Age = age
        };
        
        var response = await _client.PutAsJsonAsync($"/api/authors/{createdAuthor.AuthorId}", invalidUpdateCommand);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
        var token = await GetJwtTokenAsync();
        var createdAuthor = await CreateTestAuthorAsync();

        var response = await _client.DeleteAsync($"/api/authors/{createdAuthor.AuthorId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        result.Message.Should().Be("Author deleted successfully");
    }
    [Fact]
    public async Task DeleteAuthor_ShouldReturn_NotFound_WhenAuthorDoesNotExist()
    {
       
        var nonExistentAuthorId = Guid.NewGuid();
        
        var response = await _client.DeleteAsync($"/api/authors/{nonExistentAuthorId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
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
    
    [Fact]
    public async Task GetAuthorById_WhenAuthorDoesNotExist_ShouldReturn_NotFound()
    {
        var nonExistentAuthorId = Guid.NewGuid(); 

        var response = await _client.GetAsync($"/api/authors/{nonExistentAuthorId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }


    
    private async Task<AuthorDto> CreateTestAuthorAsync(string name = "Test Author", string email = "test@gmail.com", int age = 35)
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

        var createdAuthor = await response.Content.ReadFromJsonAsync<ApiResponse<AuthorDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return createdAuthor.Data;
    }
    
    private async Task<string> GetJwtTokenAsync()
    {
        var loginRequest = new
        {
            Email = "user@example.com",
            Password = "User123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        var json = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>( new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Raw Response: " + content);
        return json.Data.AccessToken;
    }

    

    
}