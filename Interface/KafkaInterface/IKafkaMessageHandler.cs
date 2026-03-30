using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FifoApi.Interface.KafkaInterface
{
    public interface IKafkaMessageHandler
    {
        string Topic { get; }
        Task HandleAsync(string payload);
    }
}