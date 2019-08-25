using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProductService.Queues.AMQP
{
    public class AMQPEventingConsumer : EventingBasicConsumer
    {
        public AMQPEventingConsumer(IConnectionFactory factory) : base(factory.CreateConnection().CreateModel())
        {
        }
    }
}
