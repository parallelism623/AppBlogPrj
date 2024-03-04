
using Data;
using Microsoft.EntityFrameworkCore;

namespace AppBlog.Api
{
    public static class MigrationDatabase
    {
        public static WebApplication UseMigrationDatabase(this WebApplication app)
        {
            using(var scoped = app.Services.CreateScope())
            {
                using (var context = scoped.ServiceProvider.GetRequiredService<AppBlogContext>())
                {
                    context.Database.Migrate();
                    var data = new DataSeeder();
                    data.SeedAsync(context).Wait();
                }
            }
            return app;
        }
    }
}
