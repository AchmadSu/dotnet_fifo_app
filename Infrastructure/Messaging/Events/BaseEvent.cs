using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Infrastructure.Messaging.Events
{
    public class BaseEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public string Version { get; set; } = "v1";
    }
}