using System;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.Events.Interfaces;
using OrderService.Models;
using OrderService.Repository;
using RabbitMQ.Client;

namespace OrderService.Events
{
    public class AMQPEventEmitter : IEventEmitter
    {
        private AMQPOptions _rabbitOptions;

        private ConnectionFactory _connectionFactory;

        private const string QUEUE_REKEASED_PRODUCTS = "releasedProductsDataEvent";

        private const string QUEUE_TAKEN_PRODUCTS = "takenProductsDataEvent";

        public AMQPEventEmitter(IOptions<AMQPOptions> amqpOptions, IOrderRepository productRepository)
        {
            _rabbitOptions = amqpOptions.Value;

            _connectionFactory = new ConnectionFactory();

            _connectionFactory.UserName = _rabbitOptions.Username;
            _connectionFactory.Password = _rabbitOptions.Password;
            _connectionFactory.VirtualHost = _rabbitOptions.VirtualHost;
            _connectionFactory.HostName = _rabbitOptions.HostName;
            _connectionFactory.Uri = new Uri(_rabbitOptions.Uri);
        }


        public void EmitReleasedProductsDataEvent(ReleasedProductsDataEvent releasedProductsDataEvent)
        {
            using (IConnection conn = _connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_REKEASED_PRODUCTS,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    string jsonPayload = JsonConvert.SerializeObject(releasedProductsDataEvent);
                    var body = Encoding.UTF8.GetBytes(jsonPayload);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_REKEASED_PRODUCTS,
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }

        public void EmitTakenProductsDataEvent(TakenProductsDataEvent takenProductsDataEvent)
        {
            using (IConnection conn = _connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_TAKEN_PRODUCTS,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    string jsonPayload = JsonConvert.SerializeObject(takenProductsDataEvent);
                    var body = Encoding.UTF8.GetBytes(jsonPayload);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_TAKEN_PRODUCTS,
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }
    }
}
