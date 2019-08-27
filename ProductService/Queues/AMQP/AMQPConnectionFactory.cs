using Microsoft.Extensions.Options;
using ProductService.Models;
using RabbitMQ.Client;
using System;

namespace ProductService.Queues.AMQP
{
    public class AMQPConnectionFactory : ConnectionFactory
    {
        protected AMQPOptions amqpOptions;

        public AMQPConnectionFactory(IOptions<AMQPOptions> serviceOptions) : base()
        {
            amqpOptions = serviceOptions.Value;

            UserName = amqpOptions.Username;
            Password = amqpOptions.Password;
            VirtualHost = amqpOptions.VirtualHost;
            HostName = amqpOptions.HostName;
            Uri = new Uri(amqpOptions.Uri);
        }
    }
}
