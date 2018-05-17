using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cynosura.Core.Data
{
    public interface IEntityRepository<TEntity>
    {
        IQueryable<TEntity> GetEntities();

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<List<TEntity>> ToListAsync(IQueryable<TEntity> queryable);

        Task<TEntity> FirstOrDefaultAsync(IQueryable<TEntity> queryable);

        Task<TEntity> FirstAsync(IQueryable<TEntity> queryable);

        Task<TEntity> SingleOrDefaultAsync(IQueryable<TEntity> queryable);

        Task<TEntity> SingleAsync(IQueryable<TEntity> queryable);

        Task<bool> AnyAsync(IQueryable<TEntity> queryable);

        Task<int> CountAsync(IQueryable<TEntity> queryable);
    }
}
