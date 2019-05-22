using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.DAL.Interfaces
{
    public interface IRepository<TEntity,TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(TKey id);
        Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate);
        Task<TEntity> CreateAsync(TEntity item);
        Task<TEntity> UpdateAsync(TEntity item);
        Task<TEntity> DeleteAsync(TKey id);
    }
}