using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Core.Data
{
    public static class QueryableExtensions
    {
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.ToListAsync(queryable);
        }

        public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.FirstOrDefaultAsync(queryable);
        }

        public static Task<T> FirstAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.FirstAsync(queryable);
        }

        public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.SingleOrDefaultAsync(queryable);
        }

        public static Task<T> SingleAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.SingleAsync(queryable);
        }

        public static Task<bool> AnyAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.AnyAsync(queryable);
        }

        public static Task<int> CountAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository)
        {
            return entityRepository.CountAsync(queryable);
        }
    }
}
