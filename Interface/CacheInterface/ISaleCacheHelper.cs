using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Interface.CacheInterface
{
    public interface ISaleCacheHelper
    {
        Task InvalidateListAsync();
    }
}