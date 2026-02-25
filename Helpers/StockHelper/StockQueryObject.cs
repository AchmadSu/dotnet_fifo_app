using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Helpers.StockHelper
{
    public class StockQueryObject : QueryObject
    {
        public int? QtyIn { get; set; }
        public int? QtyInOp { get; set; }
        public int? QtyRemaining { get; set; }
        public int? QtyRemainingOp { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? PurchasePriceOp { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public string? ReceivedAtOp { get; set; }

        public int? QtyInFrom { get; set; }
        public int? QtyInTo { get; set; }
        public int? QtyRemainingFrom { get; set; }
        public int? QtyRemainingTo { get; set; }
        public decimal? PurchasePriceFrom { get; set; }
        public decimal? PurchasePriceTo { get; set; }
        public DateTime? ReceivedAtFrom { get; set; }
        public DateTime? ReceivedAtTo { get; set; }
    }
}