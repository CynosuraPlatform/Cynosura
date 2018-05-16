using Cynosura.Core.Data;
using Microsoft.EntityFrameworkCore.Storage;

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

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }
    }
}
