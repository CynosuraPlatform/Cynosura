using System;
using System.Threading.Tasks;

namespace Cynosura.Core.Data
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
