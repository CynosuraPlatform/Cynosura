using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;

namespace Cynosura.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;

        protected DbContext DataContext => _databaseFactory.Get();

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public void Commit()
        {
            DataContext.SaveChanges();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await DataContext.SaveChangesAsync(cancellationToken);
        }

        public IDbTransaction BeginTransaction()
        {
            return new DbTransaction(DataContext.Database.BeginTransaction());
        }
    }
}
