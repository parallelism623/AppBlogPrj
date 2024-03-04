using AppBlog.Api.Controllers.AdminAPI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppBlog.Api.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string UrlConfirmEmail(this IUrlHelper url, string userId, string code, string scheme)
        {
            return url.Action(
                action: nameof(ProfileController.ResetPassword),
                controller: "Profile",
                values: new {code, userId},
                protocol: scheme
            );
        }
    }
}
