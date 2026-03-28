using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.DTOs.StockBatchesDTO;
using FifoApi.Models;

namespace FifoApi.Infrastructure.Messaging.Events.Sales
{
    public class SaleCreatedEvent : BaseEvent
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; } = default!;
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalQty { get; set; }
        public List<StockMovement> Movements { get; set; } = new List<StockMovement>();
    }
}