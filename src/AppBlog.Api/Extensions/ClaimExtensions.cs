using Core.Domain.Identity;
using Core.Models.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;

namespace AppBlog.Api.Extensions
{

    public static class ClaimExtensions
    {
        public static void GetPermission(this List<RoleClaimsDto> list, Type type)
        {
            FieldInfo[] Fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo f in Fields)
            {
                string displayName = f.GetValue(null).ToString();
                
                var attributes = f.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes.Length > 0)
                {
                    displayName = ((DescriptionAttribute)attributes[0]).Description;
                }
                list.Add(new RoleClaimsDto { DisplayName = displayName, Type = "Permission", Value = f.GetValue(null).ToString()});
            }
        }
        public static async Task AddPermission(this RoleManager<AppRole> app, AppRole role, string value)
        {
            var claim = new Claim("Permission",  value);
            await app.AddClaimAsync(role, claim);
        }
    }
}
