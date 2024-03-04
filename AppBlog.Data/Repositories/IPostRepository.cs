using AppBlog.Core.Domain;
using AppBlog.Core.Models.Data;
using AppBlog.Core.Models;
using AppBlog.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBlog.Data.Repositories
{
    public interface IPostRepository
    {
        public interface IPostRepository : IRepository<Post, Guid>
        {
            Task<IEnumerable<Post>> GetPopularPostAsync(int count);
            Task<PagedResult<PostInListDto>> GetPostPagingAsync(string? filter, Guid? categoryId, int pageIndex, int pageSize);
        }
    }
}
