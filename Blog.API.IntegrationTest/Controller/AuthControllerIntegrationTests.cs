using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Auths.Commands.LoginUserCommand;
using BlogApi.Application.Features.Auths.Commands.RefreshTokenCommand;
using BlogApi.Application.Features.Auths.Commands.RegisterUserCommand;
using BlogApi.Domain.Enum;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Blog.API.IntegrationTest.Controller;

public class AuthControllerIntegrationTests:IClassFixture<BlogApiWebFactory>
{
    private readonly BlogApiWebFactory _factory;
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests(BlogApiWebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
      
    }
    
    [Fact]
    public async Task RegisterUser_ShouldReturn_Success()
    {
        
        var request = new RegisterUserCommand(
            Name: "Test User",
            Email: $"testuser_{Guid.NewGuid()}@example.com", 
            Password: "StrongP@ssw0rd!"
           
        );
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var responseBody = await response.Content.ReadFromJsonAsync<Result<string>>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
        responseBody.Data.Should().Contain("User registered successfully");
    }
    
    [Fact]
    public async Task RegisterUser_ShouldFail_WhenUserAlreadyExists()
    {
        // Arrange
        var email = $"existinguser_{Guid.NewGuid()}@example.com";
        var request = new 
        {
            Name = "user",
            Email = "userauth@example.com",
            Password = "User123!",
            Role = "Maker"
        };

        
        await _client.PostAsJsonAsync("/api/auth/register", request);
        
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var responseBody = await response.Content.ReadFromJsonAsync<Result<string>>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseBody.Should().NotBeNull();
        responseBody.Error.Should().Be("User with this email already exists.");
    }
    
    [Theory]
    [InlineData("", "invalidemail@example.com", "StrongP@ssw0rd!", Role.Maker)]
    [InlineData("User", "", "StrongP@ssw0rd!", Role.Maker)]
    [InlineData("User", "not-an-email", "StrongP@ssw0rd!", Role.Maker)]
    [InlineData("User", "valid@example.com", "", Role.Maker)]
    [InlineData("User", "valid@example.com", "short", Role.Maker)]
    public async Task RegisterUser_WithInvalidInputs_ShouldReturn_BadRequest(
        string name,
        string email,
        string password,
        Role role)
    {
        
        var request = new RegisterUserCommand(name, email, password);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        var responseBody = await response.Content.ReadFromJsonAsync<Result<string>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        responseBody.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturn_Token()
    {
      

        var loginCommand = new LoginUserCommand
        {
            Email = "userauth@example.com",
            Password = "User123!"
        };
        
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginCommand);
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>();
        var responseContent = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.AccessToken.Should().NotBeNullOrEmpty();
        result.Data.RefreshToken.Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [InlineData("nonexistent@example.com", "User123!")]
    [InlineData("userauth@example.com", "WrongPassword!")]
    public async Task Login_WithInvalidCredentials_ShouldReturn_Unauthorized(string email, string password)
    {
        var loginCommand = new LoginUserCommand
        {
            Email = email,
            Password = password
        };
        
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginCommand);
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>();
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Refresh_WithValidTokens_ShouldReturn_NewTokens()
    {
        var loginRequest = new
        {
            Email = "userauth@example.com",
            Password = "User123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        var tokenResult = await loginResponse.Content.ReadFromJsonAsync<Result<TokenResponse>>();
        
        var refreshCommand = new RefreshTokenCommand
        {
            AccessToken = tokenResult.Data.AccessToken,
            RefreshToken = tokenResult.Data.RefreshToken
        };

      
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshCommand);
        var content = await response.Content.ReadAsStringAsync();
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>();

      
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.AccessToken.Should().NotBeNullOrEmpty();
        result.Data.RefreshToken.Should().NotBeNullOrEmpty();
        result.Data.RefreshToken.Should().NotBe(tokenResult.Data.RefreshToken); 
    }
    
    [Theory]
    [InlineData("invalidaccesstoken", "invalidrefreshtoken")]
    [InlineData("", "")]
    public async Task Refresh_WithInvalidTokens_ShouldReturn_BadRequests(string accessToken, string refreshToken)
    {
        var refreshCommand = new RefreshTokenCommand
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshCommand);
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResponse>>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
    }
    
    
}