using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeOptions = null, bool trackingEntities = false);
        T Get(Expression<Func<T, bool>>? filter = null, string? includeOptions = null, bool trackingEntities = false);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeOptions = null, bool trackingEntities = false);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeOptions = null, bool trackingEntities = false);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
