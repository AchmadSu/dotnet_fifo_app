using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Models;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class AdjustStockDTO
    {
        public int StockId { get; set; }
        public int Qty { get; set; }
        public int ProductId { get; set; }
        public decimal CostPrice { get; set; }
        public Guid TempSaleItemId { get; set; }
    }
}