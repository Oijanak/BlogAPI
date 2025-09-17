

using System.ComponentModel.DataAnnotations;

namespace BlogApi.Application.DTOs;

 public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get;set; } = string.Empty;
        
        public DateTime RefreshTokenExpires { get; set; }
    }

