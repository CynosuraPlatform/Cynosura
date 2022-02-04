using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cynosura.Core.Data
{
    public interface IEntityRepository<TEntity>
    {
        IQueryable<TEntity> GetEntities();

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<List<TEntity>> ToListAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);

        Task<TEntity> FirstAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);

        Task<TEntity?> SingleOrDefaultAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);

        Task<TEntity> SingleAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);

        Task<int> CountAsync(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default);
    }
}
