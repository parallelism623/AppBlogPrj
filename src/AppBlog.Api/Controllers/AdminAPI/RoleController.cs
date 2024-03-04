using AppBlog.Api.Extensions;
using AppBlog.Api.Filters;
using AutoMapper;
using Core.Domain.Identity;
using Core.Models;
using Core.Models.Data;
using Core.Models.System;
using Core.SeedWork.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AppBlog.Api.Controllers.AdminAPI
{
    [Route("api/admin/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private ILogger<RoleController> _logger;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;
        private IMemoryCache _cache;
        public RoleController(ILogger<RoleController> logger, IMemoryCache cache, IMapper mapper, RoleManager<AppRole> roleManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _cache = cache;
            _logger = logger;
        }

        
        [HttpPost("insert")]
        [ValidateModel]
        [Authorize(Permissions.Roles.View)]
        public async Task<IActionResult> InsertRole([FromBody] CreateUpdateRoleRequest request)
        {
            var role = _mapper.Map<AppRole>(request);
            await _roleManager.CreateAsync(role);
            return Ok(role);
            
        }

        [HttpPut]
        [ValidateModel]
        [Route("{id}")]
        [Authorize(Permissions.Roles.Edit)]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] CreateUpdateRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            role.DisplayName = request.DisplayName;
            role.Name = request.Name;
            await _roleManager.UpdateAsync(role);
            return Ok(role);
        }
        [HttpDelete]
        [ValidateModel]
        [Authorize(Permissions.Roles.Delete)]
        public async Task<IActionResult> DeleteRole([FromQuery] Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(role);
            return Ok(role);
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Roles.View)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpGet("paging")]
        [Authorize(Permissions.Roles.View)] 
        public async Task<ActionResult<PagedResult<RoleDto>>> GetRoleByPaging (string? filter, int pageIndex, int pageSize)
        {
            string key = "getrolepaging";
            if (_cache.TryGetValue(key, out var result))
            {
                _logger.Log(LogLevel.Information, "Find");
                return Ok(result);
            }
            else
            {
				_logger.Log(LogLevel.Information, "Not Find");
				var query = _roleManager.Roles;
                if (filter != null)
                {
                    query = query.Where(x => x.Name.Contains(filter) || x.DisplayName.Contains(filter));
                }
                var totalRow = query.Count();
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var data = await _mapper.ProjectTo<RoleDto>(query).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60)).SetPriority(CacheItemPriority.Normal);
                _cache.Set(key, data, cacheEntryOptions);
                return Ok(data);    
                
            }
        }

        [HttpGet("{id}/permissions")]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<PagedResult<PermissionDto>>> GetAllPermission(string id)
        {

            var allPermissions = new List<RoleClaimsDto>();
            Type[] nestedType = typeof(Permissions).GetNestedTypes();
            foreach (var type in nestedType)
            {
                allPermissions.GetPermission(type);
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            var permissionCurrent = ((await _roleManager.GetClaimsAsync(role)).Select(x => x.Value)).ToList();
            var allListPermissions = allPermissions.Select(x => x.Value).ToList(); 
            var totalPermission = permissionCurrent.Intersect(allListPermissions).ToList();
            foreach (var p in allPermissions)
            {
                if (totalPermission.Any(x => x == p.Value))
                {
                    p.Selected = true;
                }
            }
            var model = new PermissionDto();
            model.RoleId = id;
            model.Claims = allPermissions;
            return Ok(model);
        }
        [HttpPut("permissions")]
        [Authorize(Permissions.Roles.Edit)]
        public async Task<IActionResult> UpdateRoleClaims([FromBody] PermissionDto request)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null) 
                return NotFound();
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach(var r in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(role, r);
            }
            foreach(var r in request.Claims)
            {
                if (r.Selected)
                {
                    await _roleManager.AddPermission(role, r.Value);
                }
            }
            roleClaims = await _roleManager.GetClaimsAsync(role);
            return Ok(roleClaims);
        }
    }
}
