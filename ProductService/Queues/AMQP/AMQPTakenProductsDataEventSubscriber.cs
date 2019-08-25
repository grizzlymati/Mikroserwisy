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
        private EventingBasicConsumer consumer;
        private QueueOptions queueOptions;
        private string consumerTag;
        private IModel channel;
        public IProductRepository productRepository;

        public AMQPTakenProductsDataEventSubscriber(IOptions<QueueOptions> queueOptions,
            EventingBasicConsumer consumer, IProductRepository _productRepository)
        {
            this.queueOptions = queueOptions.Value;
            this.consumer = consumer;

            this.channel = consumer.Model;
            this.productRepository = _productRepository;

            Initialize();
        }

        private void Initialize()
        {
            channel.QueueDeclare(
                queue: queueOptions.TakenProductsDataEventEventQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);
                IEnumerable<ProductDetails> productsDetails = JsonConvert.DeserializeObject<ProductsResourcesData>(msg).ProductsDetails;
                foreach (var product in productsDetails)
                {
                    product.ProductAmount -= 2*(product.ProductAmount);
                    productRepository.UpdateProductsAmount(product);
                }
                
                channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            consumerTag = channel.BasicConsume(queueOptions.TakenProductsDataEventEventQueueName, false, consumer);
        }

        public void Unsubscribe()
        {
            channel.BasicCancel(consumerTag);
        }
    }
}
