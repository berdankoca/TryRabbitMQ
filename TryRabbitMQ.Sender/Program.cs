using System;
using RabbitMQ.Client;

namespace TryRabbitMQ.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sender started!");

            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "first-queue"
                        , durable: true
                        , exclusive: false
                        , autoDelete: false
                        , arguments: null);

                    for (int i = 0; i < 10000; i++)
                    {
                        var message = $"Hello { i }";

                        //There is a few high reliability problem in here
                        //How can I sure the message really publish in the broker?
                        //Then how can I manage transaction operation and acknowledge problem?
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        channel.BasicPublish(exchange: ""
                            , routingKey: "first-queue"
                            , basicProperties: properties
                            , body: System.Text.Encoding.UTF8.GetBytes(message));

                        Console.WriteLine("Sender: {0}", message);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
