using AppBlog.Core.SeedWork;
using AppBlog.Data.Repositories;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlog.Data.SeedWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppBlogContext _context;
        
        public UnitOfWork(AppBlogContext context, IMapper mapper)
        {
            _context = context;
            Posts = new PostRepository(context, mapper);
        }

        public IPostRepository Posts;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
