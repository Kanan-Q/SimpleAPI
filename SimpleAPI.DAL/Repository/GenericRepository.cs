using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Core.Entities.Common;
using SimpleAPI.Core.Repository;
using SimpleAPI.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        readonly AppDbContext _sql;
        readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext sql) => (_sql, _dbSet) = (sql, sql.Set<T>());

        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            await _sql.BulkInsertAsync(entities.ToList()).ConfigureAwait(false);
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity).ConfigureAwait(false);
            await _sql.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await GetByIdAsync(id).ConfigureAwait(false);
            if (data != null)
            {
                _dbSet.Remove(data);
                await _sql.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync().ConfigureAwait(false);

        public async Task<T> GetByIdAsync(int id) => await _dbSet.AsTracking().Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

        public IQueryable<T> Search(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _sql.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
