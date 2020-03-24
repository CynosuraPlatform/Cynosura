using Cynosura.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cynosura.EF.UnitTests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCynosuraEF_Success()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDatabaseFactory, FakeDatabaseFactory>();
            services.AddCynosuraEF();
            var provider = services.BuildServiceProvider();
            var unitOfWork = provider.GetService<IUnitOfWork>();
            var entityRepository = provider.GetService<IEntityRepository<SampleEntity>>();

            Assert.NotNull(unitOfWork);
            Assert.NotNull(entityRepository);
        }

        [Fact]
        public void AddCynosuraEF_OverrideEntityRepository()
        {
            var services = new ServiceCollection();
            services.AddScoped<IDatabaseFactory, FakeDatabaseFactory>();
            services.AddScoped<IEntityRepository<BaseEntity>, BaseEntityRepository>();
            services.AddCynosuraEF();
            var provider = services.BuildServiceProvider();
            var unitOfWork = provider.GetService<IUnitOfWork>();
            var entityRepository = provider.GetService<IEntityRepository<SampleEntity>>();
            var baseEntityRepository = provider.GetService<IEntityRepository<BaseEntity>>();

            Assert.NotNull(unitOfWork);
            Assert.NotNull(entityRepository);
            Assert.NotNull(baseEntityRepository);
            Assert.Equal(typeof(EntityRepository<SampleEntity>), entityRepository.GetType());
            Assert.Equal(typeof(BaseEntityRepository), baseEntityRepository.GetType());
        }

        class BaseEntity
        {

        }

        class BaseEntityRepository : IEntityRepository<BaseEntity>
        {
            public void Add(BaseEntity entity)
            {
                throw new NotImplementedException();
            }

            public Task<bool> AnyAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<int> CountAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public void Delete(BaseEntity entity)
            {
                throw new NotImplementedException();
            }

            public Task<BaseEntity> FirstAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<BaseEntity> FirstOrDefaultAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public IQueryable<BaseEntity> GetEntities()
            {
                throw new NotImplementedException();
            }

            public Task<BaseEntity> SingleAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<BaseEntity> SingleOrDefaultAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<List<BaseEntity>> ToListAsync(IQueryable<BaseEntity> queryable, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public void Update(BaseEntity entity)
            {
                throw new NotImplementedException();
            }
        }

        class SampleEntity
        {

        }

        class FakeDatabaseFactory : IDatabaseFactory
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public DbContext Get()
            {
                throw new NotImplementedException();
            }
        }
    }
}
