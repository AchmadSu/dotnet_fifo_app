using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class StockMovementDTO
    {
        public int Id { get; set; }
        public int QtyOut { get; set; }
        public decimal CostPrice { get; set; }
    }
}