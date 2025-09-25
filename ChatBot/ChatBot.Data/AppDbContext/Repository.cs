using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Data.AppDbContext
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        // New method: Search by Category ID
        public async Task<IEnumerable<T>> GetByCategoryIdAsync(int categoryId)
        {
            // Use reflection to check if the entity has a CategoryId property
            var property = typeof(T).GetProperty("CategoryId");
            if (property == null)
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a CategoryId property.");

            // Build query dynamically
            var query = _dbSet.AsQueryable();
            query = query.Where(e => (int)property.GetValue(e) == categoryId);

            return await query.ToListAsync();
        }

       
    }
}
