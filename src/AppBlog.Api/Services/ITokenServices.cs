using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppBlog.Api.Services
{
    public interface ITokenServices
    {
        Task<string?> GenerateAccessToken(IEnumerable<Claim> claims);
        Task<string?> GenerateRefreshToken();
        Task<ClaimsPrincipal?> GetClaimsPrincipal(string token);
    }
}
