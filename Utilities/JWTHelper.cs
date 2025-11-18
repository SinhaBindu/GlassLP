using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GlassLP.Utilities
{
    public class JWTHelper
    {
        public string GenerateJwtToken(string userId, string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sdffnsm656dssd$%^sddf5646566565644dsfsd4334449597743skngflsk65452252&^%"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: "yourApp",
                audience: "yourApp",
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
