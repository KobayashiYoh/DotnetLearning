using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTExampleAPI.Services
{
    public static class JWTService
    {
        public static SymmetricSecurityKey GetSecurityKey(string? keyString)
        {
            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("JWT security key is missing or empty in configuration.");
            }
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        }

        public static string GenerateToken(string key, string issuer, string audience, string userName, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
