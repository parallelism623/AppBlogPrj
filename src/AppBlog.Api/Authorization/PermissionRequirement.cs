using Microsoft.AspNetCore.Authorization;

namespace AppBlog.Api.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }   
    }
}
