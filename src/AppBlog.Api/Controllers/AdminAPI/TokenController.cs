using AppBlog.Api.Services;
using Core.Domain.Identity;
using Core.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppBlog.Api.Controllers.AdminAPI
{
    [Route("api/admin/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenServices _tokenServices;
        public TokenController(UserManager<AppUser> userManager,
                               RoleManager<AppRole> roleManager,
                               ITokenServices tokenServices) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenServices = tokenServices;
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            if (request is null)
            {
                return BadRequest("Invalid request token");
            }
            var token = request.Token;
            var refreshToken = request.RefreshToken;
            var principal = await _tokenServices.GetClaimsPrincipal(token);
            if (principal == null || principal.Identity == null || principal.Identity.Name == null)
            {
                return BadRequest("Invalid Token");
            }
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow || user.RefreshToken != refreshToken)
            {
                return BadRequest("Invalid client request");
            }
            var newToken = await _tokenServices.GenerateAccessToken(principal.Claims);
            var newRefeshToken = await _tokenServices.GenerateRefreshToken();
            user.RefreshToken = newRefeshToken;
            await _userManager.UpdateAsync(user);
            return Ok(new AuthenticatedResult
            {
                Token = newToken,
                RefreshToken = newRefeshToken,
            });
        }

        [HttpPost]
        [Route("invoke")]
        public async Task<IActionResult> InvokeToken(TokenRequest request)
        {
            var principal = await _tokenServices.GetClaimsPrincipal(request.Token);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                message = "Success",
                user = user,
            });
        }
    }
}
