using System.Text.Json;
using BlogApi.Application.DTOs;
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
        public async Task GetAllUsers_ShouldReturn_Ok()
        {
            
            var response = await _client.GetAsync("/api/users");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var users = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserDTO>>>( new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            users.Should().NotBeNull();
        }
    }

