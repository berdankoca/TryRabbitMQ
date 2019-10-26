using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TryRabbitMQ.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receiver started!");

            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            var conenction = factory.CreateConnection();
            var channel = conenction.CreateModel();
            channel.QueueDeclare(queue: "first-queue"
                , durable: true
                , exclusive: false
                , autoDelete: false
                , arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(ea.Body);
                Console.WriteLine("Receiver: {0}", message);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: "first-queue"
                , autoAck: false
                , consumer: consumer);

            Console.ReadLine();
        }
    }
}