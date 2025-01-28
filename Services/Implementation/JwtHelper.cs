using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Services.Implementation
{
    public class JwtHelper
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtHelper(IConfiguration configuration)
        {
            // Load JWT settings from appsettings.json
            _secret = configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret is missing in appsettings.json");
            _issuer = configuration["JwtSettings:Issuer"] ?? "DefaultIssuer";
            _audience = configuration["JwtSettings:Audience"] ?? "DefaultAudience";
        }

        public AuthResponseDTO GenerateToken(ApplicationUser user)
        {
            // Create claims for the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, "User") // Default role
            };

            // Generate the key from the secret
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define the token expiration
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = creds
            };

            // Create and return the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDTO
            {
                Token = tokenHandler.WriteToken(token),
                //Expiration = tokenDescriptor.Expires
            };
        }
    }
}