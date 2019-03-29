using System.Threading;
using System.Threading.Tasks;

namespace Cynosura.Core.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default);
        IDbTransaction BeginTransaction();
    }
}
