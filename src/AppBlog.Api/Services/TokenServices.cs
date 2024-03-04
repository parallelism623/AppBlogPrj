
using Core.ConfigOptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AppBlog.Api.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly JwtTokenSettings _jwtSettings;
        public TokenServices(IOptions<JwtTokenSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<string?> GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var authenKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer, 
                audience: _jwtSettings.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: authenKey
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescription);
            return tokenString;
        }

        public async Task<string?> GenerateRefreshToken()
        {
            var refresh = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(refresh);
                return Convert.ToBase64String(refresh);
            };
        }

        public async Task<ClaimsPrincipal?> GetClaimsPrincipal(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
            };
            var tokenHanlder = new JwtSecurityTokenHandler();
            var principal = tokenHanlder.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new Exception("Invalid Token");
            }
            return principal;
        }
    }
}
