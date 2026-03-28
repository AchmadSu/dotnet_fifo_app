using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Infrastructure.Messaging
{
    public static class KafkaTopics
    {
        public const string ProductCreated = "product.created";
        public const string ProductUpdated = "product.updated";

        public const string SaleCreated = "sale.created";
        public const string SaleCompleted = "sale.completed";

        public const string StockReserved = "stock.reserved";
        public const string StockUpdated = "stock.updated";
        public const string StockAdjusted = "stock.adjusted";

        public const string StockMovementCreated = "stockmovement.created";

        public static readonly string[] All = [
            ProductCreated,
            ProductUpdated,
            SaleCreated,
            SaleCompleted,
            StockReserved,
            StockUpdated,
            StockAdjusted,
            StockMovementCreated
        ];
    }
}