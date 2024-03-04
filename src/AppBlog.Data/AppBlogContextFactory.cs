using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AppBlogContextFactory : IDesignTimeDbContextFactory<AppBlogContext>
    {
        public AppBlogContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<AppBlogContext>();
            builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            return new AppBlogContext(builder.Options);
        }
    }
}
