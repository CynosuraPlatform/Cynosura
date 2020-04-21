using Cynosura.Core.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Cynosura.EF
{
    public class DbTransaction : IDbTransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;

        public DbTransaction(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public Task CommitAsync()
        {
            return _dbContextTransaction.CommitAsync();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        public Task RollbackAsync()
        {
            return _dbContextTransaction.RollbackAsync();
        }
    }
}
