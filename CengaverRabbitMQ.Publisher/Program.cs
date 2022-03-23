using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace CengaverRabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Warning = 2,
        Error = 3,
        Info = 4
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //  channel.QueueDeclare("hello-queue", false, false, false);

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Fanout);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, durable: true, false, false);
                channel.QueueBind(queueName, "logs-direct", routeKey,null);
            });

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);

                var message = $"Type Log {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine($"{x} Log sent successfully");

            });


            Console.ReadLine();
        }
    }
}
