

using System.ComponentModel.DataAnnotations;

namespace BlogApi.Application.DTOs;
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }

