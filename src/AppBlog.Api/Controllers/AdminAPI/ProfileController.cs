using AppBlog.Api.Extensions;
using AppBlog.Api.Services;
using AutoMapper;
using Core.Domain.Identity;
using Core.Models;
using Core.Models.RequestData;
using Core.SeedWork.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AppBlog.Api.Controllers.AdminAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;
        private readonly IEmailSender _email;
        public ProfileController(IMemoryCache cache, IEmailSender email, ITokenServices tokenServices, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _cache = cache;
            _email = email;
            _tokenServices = tokenServices;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("login")]
        [ModelValidExtensions]
        public async Task<IActionResult> LoginAccount([FromBody] LoginAccountRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (!result.Succeeded)
            {
                return BadRequest("Invalid user");
            }
            await _signInManager.SignInAsync(user, false);
            return Ok(new { message = "Success", firstname = user.FirstName, lastname = user.LastName });
        }
        
        [HttpPost("register")]
        [ModelValidExtensions]
        public async Task<IActionResult> RegisterAccount([FromBody] CreateAccountRequest request)
        {
            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByUserName != null || userByEmail != null)
            {
                return BadRequest("User already exists");
            }
            
            var user = _mapper.Map<AppUser>(request);
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "user");
            return Ok(new { message = "success", firstname = user.FirstName, lastname = user.LastName });
            
        }

        [HttpGet("change-password")]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> ChangePassword(string oldPassword, string password, string confirmPassword)
        {
           
            var request = new ChangePasswordRequest
            { 
                //FE
                OldPassword = oldPassword,
                Password = password,
                ConfirmPassword = confirmPassword,
            };
            return RedirectToAction("UserCreated", request);
        }
        [HttpPost("change-password")]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> ChangePassword( ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.CheckPasswordAsync(user, request.OldPassword);
            if (result)
            {
                await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);
                return Ok();
            }
            return BadRequest("Invalid password");
        }

        [HttpPost("forgotpassword")]
        [ModelValidExtensions]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound();
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.UrlConfirmEmail(user.Id.ToString(), token, Request.Scheme);
            var emailContent = new EmailSendContent
            {
                ToEmail = user.Email ?? string.Empty,
                Subject = $"Lấy lại mật khẩu",
                Content = $"Chào {user.FirstName}. Bạn vừa gửi yêu cầu lấy mật khẩu tại ...  Click: <a href='{url}'>vào đây</a> để đặt lại mật khẩu. Trân trọng."
            };
            await _email.SendEmailAsync(emailContent);
            return Ok();
            }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string code, string email)
        {
            if (code == null)
            {
                return BadRequest();
            }
            var request = new ResetPasswordModelRequest
            {
                Code = code,
                Email = email,
                OldPassword = "",
                NewPassword = "",
                ConfirmNewPassword = "",
            };
            //
            return RedirectToAction("ResetPassword",request);
        }

        [HttpPost]
        [ModelValidExtensions]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ResetPasswordAsync(user, request.Code, request.NewPassword);
            if (result.Succeeded) return Ok();
            return BadRequest();
        }
    }
}
