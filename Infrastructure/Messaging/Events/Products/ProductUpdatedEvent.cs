using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Infrastructure.Messaging.Events.Products
{
    public class ProductUpdatedEvent : BaseEvent
    {
        public int Id { get; set; }
        public string SKU { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}