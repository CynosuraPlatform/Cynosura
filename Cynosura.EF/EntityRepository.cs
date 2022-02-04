using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;

namespace Cynosura.EF
{
    public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
    {
        protected readonly IDatabaseFactory DatabaseFactory;

        protected DbContext Context => DatabaseFactory.Get();

        protected DbSet<TEntity> Entities => Context.Set<TEntity>();

        public EntityRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
        }
        
        public virtual IQueryable<TEntity> GetEntities()
        {
            return Entities;
        }

        public virtual async Task<TEntity?> GetByKeyAsync(params object[] keys)
        {
            return await Entities.FindAsync(keys);
        }

        public virtual async Task<TEntity?> GetByKeyAsync(object[] keys, CancellationToken cancellationToken)
        {
            return await Entities.FindAsync(keys, cancellationToken);
        }

        public virtual void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (Context.Entry(entity).State != EntityState.Detached)
            {

            }
            else
            {
                Entities.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State != EntityState.Detached)
            {
                Entities.Remove(entity);
            }
            else
            {
                Entities.Attach(entity);
                Context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public Task<List<TEntity>> ToListAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.ToListAsync(cancellationToken);
        }

        public Task<TEntity?> FirstOrDefaultAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<TEntity> FirstAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.FirstAsync(cancellationToken);
        }

        public Task<TEntity?> SingleOrDefaultAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.SingleOrDefaultAsync(cancellationToken);
        }

        public Task<TEntity> SingleAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.SingleAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.AnyAsync(cancellationToken);
        }

        public Task<int> CountAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default)
        {
            return queryable.CountAsync(cancellationToken);
        }
    }
}
