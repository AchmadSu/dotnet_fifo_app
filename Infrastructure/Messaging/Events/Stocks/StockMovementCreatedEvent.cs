using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Infrastructure.Messaging.Events.Stocks
{
    public class StockMovementCreatedEvent : BaseEvent
    {
        public int Id { get; set; }
        public int QtyOut { get; set; }
        public decimal CostPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public string InvoiceNo { get; set; } = default!;
        public DateTime MovementDate { get; set; }
    }
}