using System.Text.Json;
using BlogApi.Application.DTOs;
using BlogApi.Application.Features.Users.Query.LoginUserRequest;
using FluentAssertions;

namespace Blog.API.IntegrationTest.User;

using System.Net;
using System.Net.Http.Json;

using Xunit;


    public class UserControllerIntegrationTests :IClassFixture<BlogApiWebFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly BlogApiWebFactory<Program> _factory;
         public UserControllerIntegrationTests(
            BlogApiWebFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
         
         
         //Create User Test case

        [Fact]
        public async Task RegisterUser_ShouldReturn_Created()
        {
            var user = new CreateUserCommand
            {
                Name = "Test User",
                Email = "test@gmail.com",
                Password = "12345"
            };
        
            var response = await _client.PostAsJsonAsync("/api/users/register", user);

          
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdUser = await response.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>( new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            createdUser.Should().NotBeNull();
            createdUser.Data.Email.Should().Be("test@gmail.com");
        }
        
        [Fact]
        public async Task RegisterUser_ShouldReturn_BadRequest_For_Invalid_Input()
        {
           
            var user = new CreateUserCommand
            {
                Name = "Test User",
                Email = "invalid-email", 
                Password = "12345"
            };
            
            var response = await _client.PostAsJsonAsync("/api/users/register", user);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        //Get User By UserId
        [Fact]
        public async Task GetUserById_ShouldReturn_Ok()
        {
            var createUser = new CreateUserCommand
            {
                Name = "Original Name",
                Email = "original@example.com",
                Password = "12345"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users/register", createUser);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdUserResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var userId = createdUserResponse?.Data?.UserId;

            var getResponse = await _client.GetAsync($"/api/users/{userId}");
            
            var apiResponse = await getResponse.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            apiResponse.Should().NotBeNull();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Name.Should().Be("Original Name");
            apiResponse.Data.Email.Should().Be("original@example.com");

        }
        
        
        //GetAllUsers Test case

        [Fact]
        public async Task GetAllUsers_ShouldReturn_Ok()
        {
            
            var response = await _client.GetAsync("/api/users");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var users = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDTO>>>( new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            users.Should().NotBeNull();
        }

    //Update User test case      
    [Fact]
    public async Task UpdateUser_ShouldReturn_Ok_WhenUserExists_WithJwtToken()
    {
        var createUser = new CreateUserCommand
        {
            Name = "Original Name",
            Email = "original02@example.com",
            Password = "12345"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users/register", createUser);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUserResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var userId = createdUserResponse?.Data?.UserId;
    
        var loginRequest = new LoginUserRequest()
        {
            Email = createUser.Email,
            Password = createUser.Password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/users/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        string token = loginResult.Token;

    
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    
        var updateCommand = new UpdateUserRequest
        {
            Name = "Updated Name",
            Email = "updated02@example.com"
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{userId}", updateCommand);
    
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedUser = await response.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        updatedUser.Should().NotBeNull();
        updatedUser.Data.Name.Should().Be("Updated Name");
        updatedUser.Data.Email.Should().Be("updated02@example.com");
    }
    
    [Fact]
    public async Task UpdateUser_ShouldReturn_NotFound_WhenUserNotAuthorized()
    {
        var nonExistentUserId = Guid.NewGuid();

        var createUser = new CreateUserCommand
        {
            Name = "Original Name",
            Email = "original01@example.com",
            Password = "12345"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users/register", createUser);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUserResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        var loginRequest = new LoginUserRequest()
        {
            Email = createUser.Email,
            Password = createUser.Password
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/users/login", loginRequest);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        string token = loginResult.Token;

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var updateCommand = new UpdateUserRequest
        {
            Name = "Updated Name",
            Email = "updated@example.com"
        };

        var response = await _client.PatchAsJsonAsync($"/api/users/{nonExistentUserId}", updateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
    }


    
    //Delete User Test case
    [Fact]
    public async Task DeleteUser_ShouldReturn_Ok_WhenUserExists()
    {
        var createUser = new CreateUserCommand
        {
             Name = "User To Delete",
             Email = "deleteuser@example.com",
             Password = "12345"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/users/register", createUser);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUserResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponse<UserDTO>>(
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var userId = createdUserResponse.Data.UserId;
    
        var response = await _client.DeleteAsync($"/api/users/{userId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleteResult = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        deleteResult.Should().NotBeNull();
        deleteResult.Message.Should().Be("User deleted successfully");
    }
    
    [Fact]
    public async Task DeleteUser_ShouldReturn_NotFound_WhenUserDoesNotExist()
    {
        var newUserId = Guid.NewGuid();
        
        var response = await _client.DeleteAsync($"/api/users/{newUserId}");
       
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


}

  

