using System.Threading.Tasks;

namespace Cynosura.Core.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
        IDbTransaction BeginTransaction();
    }
}
