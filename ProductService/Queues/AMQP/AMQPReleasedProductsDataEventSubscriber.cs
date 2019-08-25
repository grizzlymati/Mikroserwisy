﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductService.Models;
using ProductService.Queues.Interfaces;
using ProductService.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProductService.Queues.AMQP
{
    public class AMQPReleasedProductsDataEventSubscriber : IReleasedProductsDataEventSubscriber
    {
        private EventingBasicConsumer consumer;
        private QueueOptions queueOptions;
        private string consumerTag;
        private IModel channel;
        private IProductRepository productRepository;

        public AMQPReleasedProductsDataEventSubscriber(IOptions<QueueOptions> queueOptions,
            EventingBasicConsumer consumer, IProductRepository productRepository)
        {
            this.queueOptions = queueOptions.Value;
            this.consumer = consumer;

            this.channel = consumer.Model;
            this.productRepository = productRepository;

            Initialize();
        }

        private void Initialize()
        {
            channel.QueueDeclare(
                queue: queueOptions.ReleasedProductsDataEventQueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                string msg = Encoding.UTF8.GetString(body);
                ProductDetails productDetails = JsonConvert.DeserializeObject<ProductDetails>(msg);
          //      productRepository.UpdateProductsAmount(productDetails);

                channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            consumerTag = channel.BasicConsume(queueOptions.ReleasedProductsDataEventQueueName, false, consumer);
        }

        public void Unsubscribe()
        {
            channel.BasicCancel(consumerTag);
        }
    }
}
