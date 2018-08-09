﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Core.Services.Models
{
    public static class PageModelExtensions
    {
        public static async Task<PageModel<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, IEntityRepository<T> entityRepository, int? pageIndex, int? pageSize)
        {
            var result = new PageModel<T>();
            if (pageIndex != null && pageSize != null)
            {
                result.PageItems = await queryable
                    .Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync(entityRepository);
                result.TotalItems = await queryable.CountAsync(entityRepository);
                result.CurrentPageIndex = pageIndex.Value;
            }
            else if (pageIndex == null && pageSize == null)
            {
                var items = await queryable
                    .ToListAsync(entityRepository);
                result.PageItems = items;
                result.TotalItems = items.Count;
            }
            else
            {
                throw new ArgumentException("Arguments pageIndex and pageSize must be both null or not null");
            }
            return result;
        }

        public static async Task<PageModel<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, int? pageIndex, int? pageSize)
        {
            var result = new PageModel<T>();
            if (pageIndex != null && pageSize != null)
            {
                result.PageItems = await queryable
                    .Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();
                result.TotalItems = await queryable.CountAsync();
                result.CurrentPageIndex = pageIndex.Value;
            }
            else if (pageIndex == null && pageSize == null)
            {
                var items = await queryable
                    .ToListAsync();
                result.PageItems = items;
                result.TotalItems = items.Count;
            }
            else
            {
                throw new ArgumentException("Arguments pageIndex and pageSize must be both null or not null");
            }
            return result;
        }

        public static async Task<PageModel<TDst>> MapToPagedListAsync<TSrc, TDst>(this IQueryable<TSrc> queryable, IEntityRepository<TSrc> entityRepository,  IMapper mapper, int pageIndex, int pageSize)
        {
            var items = await queryable
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(entityRepository);
            var mapped = items.Select(mapper.Map<TSrc, TDst>);
            var result = new PageModel<TDst>(mapped, await queryable.CountAsync(entityRepository), pageIndex);
            return result;
        }

        public static async Task<PageModel<TDst>> MapToPagedListAsync<TSrc, TDst>(this IQueryable<TSrc> queryable, IMapper mapper, int pageIndex, int pageSize)
        {
            var items = await queryable
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var mapped = items.Select(mapper.Map<TSrc, TDst>);
            var result = new PageModel<TDst>(mapped, await queryable.CountAsync(), pageIndex);
            return result;
        }

        public static PageModel<TDst> Map<TSrc, TDst>(this PageModel<TSrc> model, IMapper mapper)
        {
            return new PageModel<TDst>(model.PageItems.Select(mapper.Map<TSrc, TDst>), model.TotalItems, model.CurrentPageIndex);
        }
    }
}
