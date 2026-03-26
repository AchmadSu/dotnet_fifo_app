using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Interface.CacheInterface
{
    public interface IProductCacheHelper
    {
        Task InvalidateListAsync();
        Task InvalidateDetailAsync(int id);
        Task InvalidateSKUAsync(string sku);
        Task InvalidateAllAsync(int id, string sku);
    }
}