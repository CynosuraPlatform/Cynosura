using System.Threading.Tasks;
using Cynosura.Core.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task CommitAsync()
        {
            await DataContext.SaveChangesAsync();
        }

        public IDbTransaction BeginTransaction()
        {
            return new DbTransaction(DataContext.Database.BeginTransaction());
        }
    }
}
