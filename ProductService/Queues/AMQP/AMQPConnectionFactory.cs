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
            this.amqpOptions = serviceOptions.Value;

            this.UserName = amqpOptions.Username;
            this.Password = amqpOptions.Password;
            this.VirtualHost = amqpOptions.VirtualHost;
            this.HostName = amqpOptions.HostName;
            this.Uri = new Uri(amqpOptions.Uri);
        }
    }
}
