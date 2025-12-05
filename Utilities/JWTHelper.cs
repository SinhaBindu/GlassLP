using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static GlassLP.Data.Service;

namespace GlassLP.Utilities
{
	public class JWTHelper
	{
		public string GenerateJwtToken(GlobalDataService globalData)
		{
			var secretKey = "sdffnsm656dssd$%^sddf5646566565644dsfsd4334449597743skngflsk65452252&^%";
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = GenerateClaims(globalData);
			var token = new JwtSecurityToken(
				issuer: "https://yourdomain.com",
				audience: "https://yourdomain.com",
				claims: claims,
				expires: DateTime.Now.AddHours(4),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public List<Claim> GenerateClaims(GlobalDataService globalData)
		{
			var claims = new List<Claim>
			{

				new Claim("UserId",  globalData.UserId),
				new Claim(ClaimTypes.NameIdentifier,  globalData.UserId),
				new Claim(ClaimTypes.Name,  globalData.UserName),
				new Claim(ClaimTypes.Role,  globalData.Role),
					new Claim("PhoneNumber", globalData.PhoneNumber),
					new Claim("Email", globalData.Email),
					new Claim("RoleId", globalData.RoleId),
					new Claim("DistrictIds", globalData.DistrictIds),
					new Claim("DistrictName", globalData.DistrictName),
					new Claim("BlockId", globalData.BlockId),
					new Claim("BlockName", globalData.BlockName),
					new Claim("CLFId", globalData.CLFId),
					new Claim("CLFName", globalData.CLFName),
					new Claim("LoginTime", globalData.LoginTime)

			};
			return claims;
		}
	}
}
