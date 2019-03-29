using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Core.Data
{
    public static class EntityRepositoryExtensions
    {
        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IEntityRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            repository.GetEntities().FirstOrDefaultAsync(predicate, cancellationToken);

        public static Task<TEntity> FirstAsync<TEntity>(this IEntityRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            repository.GetEntities().FirstAsync(predicate, cancellationToken);

        public static Task<TEntity> SingleOrDefaultAsync<TEntity>(this IEntityRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            repository.GetEntities().SingleOrDefaultAsync(predicate, cancellationToken);

        public static Task<TEntity> SingleAsync<TEntity>(this IEntityRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            repository.GetEntities().SingleAsync(predicate, cancellationToken);

        public static Task<bool> AnyAsync<TEntity>(this IEntityRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            repository.GetEntities().AnyAsync(predicate, cancellationToken);

        public static Task<int> CountAsync<TEntity>(this IEntityRepository<TEntity> repository,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            repository.GetEntities().CountAsync(predicate, cancellationToken);

        public static Task<TEntity> FindAsync<TEntity>(this IEntityRepository<TEntity> repository,
            params object[] keys)
            where TEntity : class =>
            repository.GetDbSet().FindAsync(keys);

        public static Task<TEntity> FindAsync<TEntity>(this IEntityRepository<TEntity> repository,
            object[] keys, CancellationToken cancellationToken)
            where TEntity : class =>
            repository.GetDbSet().FindAsync(keys, cancellationToken);

        private static DbSet<TEntity> GetDbSet<TEntity>(this IEntityRepository<TEntity> repository)
            where TEntity : class =>
            repository.GetEntities() as DbSet<TEntity>;
    }
}
