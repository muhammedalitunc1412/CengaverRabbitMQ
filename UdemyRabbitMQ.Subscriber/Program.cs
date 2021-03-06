using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Subscriber
{
   
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            // channel.QueueDeclare("hello-queue", false, false, false);

            channel.BasicQos(0, 1, false);

            //var randomQueueName = channel.QueueDeclare().QueueName;

            // channel.QueueDeclare(randomQueueName, true, false, false);


          //  channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            var consumer = new EventingBasicConsumer(channel);
            var queueName = "direct-queue-Critical";
            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("Listening logs");
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
             {
                 var message = Encoding.UTF8.GetString(e.Body.ToArray());

                 Console.WriteLine($"Message is: {message}");

                 File.AppendAllText("log-critical.txt",message+"\n");

                 channel.BasicAck(e.DeliveryTag, false);
             };

            Console.ReadLine();
        }
    }
}
