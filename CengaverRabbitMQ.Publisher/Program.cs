using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace CengaverRabbitMQ.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("hello-queue", false, false, false);
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                var message = $"{x}. Message";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"{x}. Message sent successfully");

            });


            Console.ReadLine();
        }
    }
}
