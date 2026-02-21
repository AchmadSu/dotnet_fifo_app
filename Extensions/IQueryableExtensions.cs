using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FifoApi.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResult<TDestination>> ToPagedResultAsync<TSource, TDestination>(
            this IQueryable<TSource> query,
            int pageNumber,
            int pageSize,
            Func<TSource, TDestination> mapFunc
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 20 : (pageSize > 50 ? 50 : pageSize);

            var totalCount = await query.CountAsync();

            var skipNumber = (pageNumber - 1) * pageSize;
            var items = await query
                .Skip(skipNumber)
                .Take(pageSize)
                .ToListAsync();

            var dtoItems = items.Select(mapFunc).ToList();

            return new PagedResult<TDestination>
            {
                Items = dtoItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}