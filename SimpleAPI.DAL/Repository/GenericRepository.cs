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

namespace SimpleAPI.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    readonly AppDbContext _sql; readonly DbSet<T> _dbSet;

    #region Constructor
    public GenericRepository(AppDbContext sql) => (_sql, _dbSet) = (sql, sql.Set<T>());
    #endregion Constructor

    #region BulkInsertAsync
    public async Task BulkInsertAsync(IEnumerable<T> entities) => await _sql.BulkInsertAsync(entities.ToList()).ConfigureAwait(false);
    #endregion BulkInsertAsync


    #region CreateAsync
    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await _sql.SaveChangesAsync().ConfigureAwait(false);
    }
    #endregion CreateAsync


    #region DeleteAsync
    public async Task DeleteAsync(int id)
    {
        var data = await GetByIdAsync(id).ConfigureAwait(false);
        if (data != null)
        {
            _dbSet.Remove(data);
            await _sql.SaveChangesAsync().ConfigureAwait(false);
        }
    }
    #endregion DeleteAsync

    #region GetAllAsync
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync().ConfigureAwait(false);
    #endregion GetAllAsync

    #region GetByIdAsync
    public async Task<T> GetByIdAsync(int id) => await _dbSet.AsTracking().Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

    #endregion GetByIdAsync


    #region Search
    public IQueryable<T> Search(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);
    #endregion Search


    #region UpdateAsync
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _sql.SaveChangesAsync().ConfigureAwait(false);
    }
    #endregion UpdateAsync
}

