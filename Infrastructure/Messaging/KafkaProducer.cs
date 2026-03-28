using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using FifoApi.Interface.KafkaInterface;

namespace FifoApi.Infrastructure.Messaging
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        public KafkaProducer(IConfiguration config)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }
        public async Task ProduceAsync<T>(string topic, T message)
        {
            var json = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(topic,
                new Message<Null, string>
                {
                    Value = json
                }
            );
        }
    }
}