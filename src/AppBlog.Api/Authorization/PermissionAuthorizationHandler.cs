using Core.Domain.Identity;
using Core.SeedWork.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Core.SeedWork.Constants.Permissions;


namespace AppBlog.Api.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public PermissionAuthorizationHandler(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
 
            if (context.User.Identity.IsAuthenticated == false)
            {
                context.Fail();
                return;
            }
            var user = await _userManager.FindByNameAsync(context.User.Identity.Name);
            var role = await _userManager.GetRolesAsync(user);


            if (role.Contains(Core.SeedWork.Constants.Roles.Admin))
            {
                context.Succeed(requirement);
                return;
            }

            var allPermissions = new List<Claim>();
            foreach(var r in role)
            {
                var rr = await _roleManager.FindByNameAsync(r);
                var roleClaim = await _roleManager.GetClaimsAsync(rr);
                allPermissions.AddRange(roleClaim);
            }
            var permission = allPermissions.Where(x => x.Type == "Permission" && x.Value == requirement.Permission);
            if (permission.Any())
            {
                context.Succeed(requirement);
                return;
            }

        }
    }
}
