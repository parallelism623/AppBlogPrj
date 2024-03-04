using AutoMapper;
using Core.Domain.Identity;
using Core.Models;
using Core.Models.Data;
using Core.Models.RequestData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Core.SeedWork.Constants.Permissions;

namespace AppBlog.Api.Controllers.AdminAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> userManager, IMapper mapper) {
            _userManager = userManager; 
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize(Users.View)]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }
        [HttpGet]
        [Authorize(Users.View)]
        public async Task<ActionResult<PagedResult<UserDto>>> GetPaging(string? keyword, int pageIndex, int pageSize)
        {
            var query = _userManager.Users;
            if (keyword != null)
            {
                query = query.Where(x => x.FirstName.Contains(keyword)
                                         || x.UserName.Contains(keyword)
                                         || x.Email.Contains(keyword)
                                         || x.PhoneNumber.Contains(keyword));
            }
            var totalRows = await query.CountAsync();
            var items = query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var results = await _mapper.ProjectTo<UserDto>(items).ToListAsync();
            return Ok(new PagedResult<UserDto>
            {
                Results = results,
                RowCount = totalRows,
                PageSize = pageSize,
                CurrentPage = pageIndex
            });
        }
        [HttpPost]
        [Authorize(Users.Create)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var useByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (useByUserName != null)
            {
                return BadRequest("Username have already exist");
            }
            var userByEmail = await _userManager.FindByIdAsync(request.Email);
            if (userByEmail != null)
            {
                return BadRequest("Email have already exist");
            }
            var newUser = _mapper.Map<AppUser>(request);
            await _userManager.CreateAsync(newUser, request.Password);
            return Ok(new {message = "Tao thanh cong"});
        }


    }
}
