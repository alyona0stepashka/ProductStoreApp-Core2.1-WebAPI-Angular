using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.DAL.Interfaces
{
    public interface IFileRepository<TEntity> 
    {
        Task<TEntity> CreateAsync(TEntity item);
        Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate);
    }
}