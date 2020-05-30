using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Cynosura.Core.Data;

namespace Cynosura.Core.Services.Models
{
    public static class PageModelExtensions
    {
        public static async Task<PageModel<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, int? pageIndex, int? pageSize, CancellationToken cancellationToken = default)
        {
            var result = new PageModel<T>();
            if (pageIndex != null && pageSize != null)
            {
                result.PageItems = await queryable
                    .Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync(entityRepository, cancellationToken);
                result.TotalItems = await queryable.CountAsync(entityRepository, cancellationToken);
                result.CurrentPageIndex = pageIndex.Value;
            }
            else if (pageIndex == null && pageSize == null)
            {
                var items = await queryable
                    .ToListAsync(entityRepository, cancellationToken);
                result.PageItems = items;
                result.TotalItems = items.Count;
            }
            else
            {
                throw new ArgumentException("Arguments pageIndex and pageSize must be both null or not null");
            }
            return result;
        }

        public static async Task<PageModel<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, int? pageIndex, int? pageSize, CancellationToken cancellationToken = default)
        {
            var result = new PageModel<T>();
            if (pageIndex != null && pageSize != null)
            {
                result.PageItems = await queryable
                    .Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync(cancellationToken);
                result.TotalItems = await queryable.CountAsync(cancellationToken);
                result.CurrentPageIndex = pageIndex.Value;
            }
            else if (pageIndex == null && pageSize == null)
            {
                var items = await queryable
                    .ToListAsync(cancellationToken);
                result.PageItems = items;
                result.TotalItems = items.Count;
            }
            else
            {
                throw new ArgumentException("Arguments pageIndex and pageSize must be both null or not null");
            }
            return result;
        }

        public static async Task<PageModel<TDst>> MapToPagedListAsync<TSrc, TDst>(this IQueryable<TSrc> queryable, IEntityRepository<TSrc> entityRepository,  IMapper mapper, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var items = await queryable
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(entityRepository, cancellationToken);
            var totalItems = await queryable.CountAsync(entityRepository, cancellationToken);
            var mapped = items.Select(mapper.Map<TSrc, TDst>);
            var result = new PageModel<TDst>(mapped, totalItems, pageIndex);
            return result;
        }

        public static async Task<PageModel<TDst>> MapToPagedListAsync<TSrc, TDst>(this IQueryable<TSrc> queryable, IMapper mapper, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var items = await queryable
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            var totalItems = await queryable.CountAsync(cancellationToken);
            var mapped = items.Select(mapper.Map<TSrc, TDst>);
            var result = new PageModel<TDst>(mapped, totalItems, pageIndex);
            return result;
        }

        public static PageModel<TDst> Map<TSrc, TDst>(this PageModel<TSrc> model, IMapper mapper)
        {
            return new PageModel<TDst>(model.PageItems.Select(mapper.Map<TSrc, TDst>), model.TotalItems, model.CurrentPageIndex);
        }
    }
}
