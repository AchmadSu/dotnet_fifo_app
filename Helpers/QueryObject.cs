using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Helpers
{
    public class QueryObject
    {
        private int _pageSize = 20;
        public string? Search { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 50) ? 50 : (value < 1 ? 1 : value);
        }
    }
}