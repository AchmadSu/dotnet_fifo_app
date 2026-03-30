using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FifoApi.Interface.KafkaInterface;

namespace FifoApi.Infrastructure.Messaging
{
    public class KafkaMessageDispatcher
    {
        private readonly Dictionary<string, IKafkaMessageHandler> _handlers;

        public KafkaMessageDispatcher(IEnumerable<IKafkaMessageHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.Topic);
        }

        public async Task DispatchAsync(string topic, string payload)
        {
            if (_handlers.TryGetValue(topic, out var handler))
            {
                await handler.HandleAsync(payload);
                return;
            }

            throw new Exception($"No handler registered for topic {topic}");
        }
    }
}