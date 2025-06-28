using Microsoft.EntityFrameworkCore;
using SimpleAPI.Core.Entities.Common;
using SimpleAPI.Core.Repository;
using SimpleAPI.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        readonly AppDbContext _sql;
        readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext sql) => (_sql, _dbSet) = (sql, sql.Set<T>());

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _sql.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await GetByIdAsync(id);
            if (data != null)
            {
                _dbSet.Remove(data);
                await _sql.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _dbSet.AsTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _sql.SaveChangesAsync();
        }
    }
}
