using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class AdjustStockDTO
    {
        public int Id { get; set; }
        public int Qty { get; set; }
    }
}