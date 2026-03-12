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
        public decimal UnitPrice { get; set; }
        public string? InvoiceNo { get; set; } = string.Empty;
        public DateTime? MovementDate { get; set; }
    }
}