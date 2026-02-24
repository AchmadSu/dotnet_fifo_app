using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Helpers.StockHelper
{
    public class StockQueryObject : QueryObject
    {
        public int QtyIn { get; set; }
        public int QtyRemaining { get; set; }
    }
}