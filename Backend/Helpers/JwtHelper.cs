using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Models;

namespace Backend.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var keyVal = _config["Jwt:Key"];

            // Check your terminal during Login!
            Console.WriteLine("--- JWT HELPER CONFIG CHECK ---");
            Console.WriteLine($"Issuer:   '{issuer}'");
            Console.WriteLine($"Audience: '{audience}'");
            Console.WriteLine($"Key Length: {(keyVal?.Length ?? 0)} characters");
            Console.WriteLine("-------------------------------");
            
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyVal!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                // Use JwtRegisteredClaimNames.Sub for standard "sub"
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}