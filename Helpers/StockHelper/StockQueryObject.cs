using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Validations;

namespace FifoApi.Helpers.StockHelper
{
    public class StockQueryObject : QueryObject
    {
        public int? QtyIn { get; set; }

        [ComparisonOperator]
        public string? QtyInOp { get; set; }

        public int? QtyRemaining { get; set; }

        [ComparisonOperator]
        public string? QtyRemainingOp { get; set; }

        public decimal? PurchasePrice { get; set; }

        [ComparisonOperator]
        public string? PurchasePriceOp { get; set; }

        public DateTime? ReceivedAt { get; set; }

        [ComparisonOperator]
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