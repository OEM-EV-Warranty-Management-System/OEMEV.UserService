using Microsoft.IdentityModel.Tokens;
using OEMEV.UserService.Domain;
using OEMEV.UserService.Infrastructure.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OEMEV.UserService.Infrastructure.Libraries
{
	public static class Authentication
	{
		public static (string? token, string? error) CreateAccessToken(User user, JWTSettings jwtSettings)
		{
			try
			{
				if (string.IsNullOrEmpty(jwtSettings.SecretKey))
					throw new ArgumentNullException(nameof(jwtSettings.SecretKey), "Secret key is missing.");

				if (jwtSettings.SecretKey.Length < 32)
					throw new ArgumentException("Secret key must be at least 32 characters long for HMAC-SHA256 security.");

				var now = DateTime.UtcNow;

				var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, user.RoleId.ToString()),
				new Claim("token_type", "access"),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat,
				new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
			};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var token = new JwtSecurityToken(
					issuer: jwtSettings.Issuer,
					audience: jwtSettings.Audience,
					claims: claims,
					notBefore: now,
					expires: now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
					signingCredentials: creds
				);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);
				return (jwt, null);
			}

			catch (Exception ex)
			{
				return (string.Empty, $"DAL.Libraries.Authentication.CreateAccessToken Error: {ex.Message}");
			}
		}

		public static (string? token, string? error) CreateRefreshToken(User user, JWTSettings jwtSettings)
		{
			try
			{
				if (string.IsNullOrEmpty(jwtSettings.SecretKey))
					throw new ArgumentNullException(nameof(jwtSettings.SecretKey), "Secret key is missing.");

				if (jwtSettings.SecretKey.Length < 32)
					throw new ArgumentException("Secret key must be at least 32 characters long for HMAC-SHA256 security.");

				var now = DateTime.UtcNow;

				var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Role, user.RoleId.ToString()),
				new Claim("token_type", "refresh"),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat,
				new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
			};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				var token = new JwtSecurityToken(
					issuer: jwtSettings.Issuer,
					audience: jwtSettings.Audience,
					claims: claims,
					notBefore: now,
					expires: now.AddDays(jwtSettings.RefreshTokenExpirationDays),
					signingCredentials: creds
				);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);
				return (jwt, null);
			}
			catch (Exception ex)
			{
				return (string.Empty, $"DAL.Libraries.Authentication.CreateRefreshToken Error: {ex.Message}");
			}
		}

		public static string? GetTokenType(this ClaimsPrincipal user)
		{
			return user.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value ?? string.Empty;
		}

		public static (ClaimsPrincipal? principal, string? error) ValidateToken(string token, JWTSettings jwtSettings, bool validateLifetime = true)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();

				if (string.IsNullOrEmpty(jwtSettings.SecretKey))
					throw new ArgumentNullException(nameof(jwtSettings.SecretKey), "Secret key is missing.");

				var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
				
				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidateAudience = true,
					ValidAudience = jwtSettings.Audience,
					ValidateLifetime = validateLifetime,
					ClockSkew = TimeSpan.Zero 
				};
				
				var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
				
				if (validatedToken is JwtSecurityToken jwtToken &&
					jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				{
					return (principal, null);
				}
					throw new SecurityTokenException("Invalid token algorithm.");
			}
			catch (Exception ex)
			{
				return (null, $"DAL.Libraries.Authentication.ValidateToken: {ex.Message}"); 
			}
		}
	}
}
