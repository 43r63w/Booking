using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> _set;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public T Get(Expression<Func<T, bool>>? filter = null,
            string? includeOptions = null,
            bool trackingEntities = false)
        {
            IQueryable<T> query = _set;

            if (query != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeOptions))
            {
                foreach (var options in includeOptions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(options);
                }
            }

            if (trackingEntities)
            {
                return query.FirstOrDefault();
            }

          return query.AsNoTracking().FirstOrDefault();

        }

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter = null, string?
            includeOptions = null,
            bool trackingEntities = false)
        {
            IQueryable<T> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeOptions))
            {
                foreach (var options in includeOptions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(options);
                }
            }

            if (trackingEntities)
            {
                return query.ToList();
            }

            return query.AsNoTracking().ToList();

        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeOptions = null,
            bool trackingEntities = false)
        {
            IQueryable<T> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (!string.IsNullOrWhiteSpace(includeOptions))
            {
                foreach (var options in includeOptions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(options);
                }
            }

            if (trackingEntities)
            {
                return await query.ToListAsync();
            }

            return await query.AsNoTracking().ToListAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, 
            string? includeOptions = null,
            bool trackingEntities = false)
        {
            IQueryable<T> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeOptions))
            {
                foreach (var options in includeOptions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(options);
                }
            }

            if (trackingEntities)
            {
                return await query.FirstOrDefaultAsync();
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
