using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.DTOs.StockBatchesDTO
{
    public class StockDTO
    {
        public int Id { get; set; }
        public int QtyIn { get; set; }
        public int QtyRemaining { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime ReceivedAt { get; set; }
        public int? ProductId { get; set; }
    }
}