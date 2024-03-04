using Core.Domain.Identity;

using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class DataSeeder
    {
        public async Task SeedAsync(AppBlogContext context)
        {
            var passWordHasher = new PasswordHasher<AppUser>();
            var adminRoleId = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                await context.AddAsync(new AppRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "Quản trị viên"
                });
                await context.SaveChangesAsync();
            }
            var adminUserId = Guid.NewGuid();
            if (!context.Users.Any())
            {
                var user = new AppUser
                {
                    Id = adminUserId,
                    FirstName = "Hieu",
                    LastName = "Truong",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now
                };
                user.PasswordHash = passWordHasher.HashPassword(user, "Admin@123");
                await context.Users.AddAsync(user);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId,
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
