using System;

namespace Cynosura.Core.Data
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
