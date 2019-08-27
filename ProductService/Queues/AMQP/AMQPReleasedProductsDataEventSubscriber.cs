using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductService.Models;
using ProductService.Queues.Interfaces;
using ProductService.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Queues.AMQP
{
    public class AMQPReleasedProductsDataEventSubscriber : IReleasedProductsDataEventSubscriber
    {
        private EventingBasicConsumer _consumer;
        private QueueOptions _queueOptions;
        private string _consumerTag;
        private IModel _channel;
        private IProductRepository _productRepository;

        public AMQPReleasedProductsDataEventSubscriber(IOptions<QueueOptions> queueOptions,
            EventingBasicConsumer consumer, IProductRepository productRepository)
        {
            _queueOptions = queueOptions.Value;
            _consumer = consumer;
            _channel = consumer.Model;
            _productRepository = productRepository;

            Initialize();
        }

        private void Initialize()
        {
            _channel.QueueDeclare(
                queue: _queueOptions.ReleasedProductsDataEventQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                string msg = Encoding.UTF8.GetString(body);
                if (msg != null && msg != "null")
                {
                    IEnumerable<ProductDetails> productsDetails = JsonConvert.DeserializeObject<ProductsResourcesData>(msg).ProductsDetails;
                    foreach (var product in productsDetails)
                    {
                        _productRepository.UpdateProductsAmount(product);
                    }
                }
                _channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            _consumerTag = _channel.BasicConsume(_queueOptions.ReleasedProductsDataEventQueueName, false, _consumer);
        }

        public void Unsubscribe()
        {
            _channel.BasicCancel(_consumerTag);
        }
    }
}
