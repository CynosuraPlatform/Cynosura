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
    }
}
