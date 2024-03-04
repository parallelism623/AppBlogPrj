
using AutoMapper;
using Core.Repostitories;
using Core.SeedWork;
using Data.Repostitories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppBlogContext _context;

        public UnitOfWork(AppBlogContext context, IMapper mapper)
        {
            _context = context;
            Posts = new PostRepository(context, mapper);
        }
        public IPostRepository Posts { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
