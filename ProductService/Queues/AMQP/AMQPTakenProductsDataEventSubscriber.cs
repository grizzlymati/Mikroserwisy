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
    public class AMQPTakenProductsDataEventSubscriber : ITakenProductsDataEventSubscriber
    {
        private EventingBasicConsumer _consumer;
        private QueueOptions _queueOptions;
        private string _consumerTag;
        private IModel _channel;
        private IProductRepository _productRepository;

        public AMQPTakenProductsDataEventSubscriber(IOptions<QueueOptions> queueOptions,
            EventingBasicConsumer consumer, IProductRepository productRepository)
        {
            _queueOptions = queueOptions.Value;
            _consumer = consumer;
            _channel = _consumer.Model;
            _productRepository = productRepository;

            Initialize();
        }

        private void Initialize()
        {
            _channel.QueueDeclare(
                queue: _queueOptions.TakenProductsDataEventEventQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);
                if (msg != null && msg != "null")
                {
                    IEnumerable<ProductDetails> productsDetails = JsonConvert.DeserializeObject<ProductsResourcesData>(msg).ProductsDetails;
                    foreach (var product in productsDetails)
                    {
                        product.ProductAmount -= 2 * (product.ProductAmount);
                        _productRepository.UpdateProductsAmount(product);
                    }
                }
                _channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            _consumerTag = _channel.BasicConsume(_queueOptions.TakenProductsDataEventEventQueueName, false, _consumer);
        }

        public void Unsubscribe()
        {
            _channel.BasicCancel(_consumerTag);
        }
    }
}
