using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Core.Data
{
    public static class QueryableExtensions
    {
        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.ToListAsync(queryable, cancellationToken);
        }

        public static Task<T?> FirstOrDefaultAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.FirstOrDefaultAsync(queryable, cancellationToken);
        }

        public static Task<T> FirstAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.FirstAsync(queryable, cancellationToken);
        }

        public static Task<T?> SingleOrDefaultAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.SingleOrDefaultAsync(queryable, cancellationToken);
        }

        public static Task<T> SingleAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.SingleAsync(queryable, cancellationToken);
        }

        public static Task<bool> AnyAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.AnyAsync(queryable, cancellationToken);
        }

        public static Task<int> CountAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, CancellationToken cancellationToken = default)
        {
            return entityRepository.CountAsync(queryable, cancellationToken);
        }
    }
}
