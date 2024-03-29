﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace QingTian.Core
{
    /// <summary>
    /// 分页拓展类
    /// </summary>
    public static class PagedQueryableExtensions
    {
        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<SqlSugarPagedList<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize)
        {
            RefAsync<int> totalCount = 0;
            var items = await query.ToPageListAsync(pageIndex, pageSize, totalCount);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new SqlSugarPagedList<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items,
                TotalCount = (int)totalCount,
                TotalPages = totalPages,
                HasNextPages = pageIndex < totalPages,
                HasPrevPages = pageIndex - 1 > 0
            };
        }
        public static async Task<SqlSugarPagedList<TEntity>> ToPagedListAsync<TEntity>(this IList<TEntity> dataList, int pageIndex, int pageSize)
        {
           return await Task.Run(() =>
            {
                RefAsync<int> totalCount = dataList.Count;
                var items = dataList.Skip((pageIndex-1) * pageSize).Take(pageSize);
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                return new SqlSugarPagedList<TEntity>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Items = items,
                    TotalCount = (int)totalCount,
                    TotalPages = totalPages,
                    HasNextPages = pageIndex < totalPages,
                    HasPrevPages = pageIndex - 1 > 0
                };
            });
        }
    }
}