using Core.Repostitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeedWork
{
    public interface IUnitOfWork
    {
        
        IPostRepository Posts { get; }
        Task<int> CompleteAsync();
    }
}
