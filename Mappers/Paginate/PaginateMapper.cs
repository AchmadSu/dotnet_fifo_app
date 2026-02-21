using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs;

namespace FifoApi.Mappers.Paginate
{
    public static class PaginateMapper
    {
        public static PagedResult<TDestination> Map<TSource, TDestination>(
            this PagedResult<TSource> source,
            Func<TSource, TDestination> mapFunc
        )
        {
            return new PagedResult<TDestination>
            {
                Items = source.Items.Select(mapFunc).ToList(),
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount
            };
        }
    }
}