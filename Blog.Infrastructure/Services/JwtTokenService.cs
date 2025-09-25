using BlogApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BlogApi.Application.Exceptions;

namespace BlogApi.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt Key is not found")) );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
          

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:ExpireMinutes"] ?? "5")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateLifetime = false, 
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals
                    (SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApiException("Invalid token",HttpStatusCode.Unauthorized);
            }
            
            return principal;
        }
    }
}
