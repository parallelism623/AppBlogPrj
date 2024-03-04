using AppBlog.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlog.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync (AppBlogContext context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            var rootAdminRoleId = Guid.NewGuid();
            if (!context.Roles.Any()) {
                await context.Roles.AddAsync(new AppRole
                {
                    Id = rootAdminRoleId,
                    Name = "admin",
                    DisplayName = "Quản trị",
                    NormalizedName = "ADMIN"
                });
                await context.SaveChangesAsync();  
            }
            var rootAdminUserId = Guid.NewGuid();
            if (!context.Users.Any())
            {
                var user = new AppUser
                {
                    Id = rootAdminUserId,
                    FirstName = "admin",
                    LastName = "admin",
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");
                await context.AddAsync(user);
                await context.AddAsync(new IdentityUserRole<Guid>
                {
                    RoleId = rootAdminRoleId,
                    UserId = rootAdminUserId,
                });
                await context.SaveChangesAsync();   
            }
            
        }
    }
}
