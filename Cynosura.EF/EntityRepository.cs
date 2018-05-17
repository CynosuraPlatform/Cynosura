using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Microsoft.EntityFrameworkCore;

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

        public virtual async Task<TEntity> GetByKeyAsync(params object[] keys)
        {
            return await Entities.FindAsync(keys);
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

        public Task<List<TEntity>> ToListAsync(IQueryable<TEntity> queryable)
        {
            return queryable.ToListAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> queryable)
        {
            return queryable.FirstOrDefaultAsync();
        }

        public Task<TEntity> FirstAsync(IQueryable<TEntity> queryable)
        {
            return queryable.FirstAsync();
        }

        public Task<TEntity> SingleOrDefaultAsync(IQueryable<TEntity> queryable)
        {
            return queryable.SingleOrDefaultAsync();
        }

        public Task<TEntity> SingleAsync(IQueryable<TEntity> queryable)
        {
            return queryable.SingleAsync();
        }

        public Task<bool> AnyAsync(IQueryable<TEntity> queryable)
        {
            return queryable.AnyAsync();
        }

        public Task<int> CountAsync(IQueryable<TEntity> queryable)
        {
            return queryable.CountAsync();
        }
    }
}
