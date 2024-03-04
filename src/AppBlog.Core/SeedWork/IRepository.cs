using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeedWork
{
    public interface IRepository <T, Key> where T : class 
    {
        Task<T> GetByIdAsync(Key id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);   
        void Add(T entity); 
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);  
        void RemoveRange(IEnumerable<T> entities);
    }
}
