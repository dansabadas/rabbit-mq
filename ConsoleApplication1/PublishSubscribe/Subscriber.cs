using System;
using RabbitMQ.Client;
using ConsoleApplication1.Common;
using System.Threading.Tasks;

namespace RabbitMQ.Examples
{
    public class Subscriber
    {
        private const string ExchangeName = "PublishSubscribe_Exchange";

        public static void RunInParallel()
        {
            Task.Run(() => Run());
            Task.Run(() => Run());
            Console.ReadLine();
        }

        public static void Run()
        {
            ConnectionFactory factory = RabbitMQConnectionFactory.GetFactory(isRemote: false);
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    QueueingBasicConsumer consumer;
                    var queueName = DeclareAndBindQueueToExchange(channel, out consumer);
                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var message = (Payment)ea.Body.DeSerialize(typeof(Payment));

                        Console.WriteLine("{2}----- Payment Processed {0} : {1}", message.CardNumber, message.AmountToPay, queueName);
                    }
                }
            }
        }

        private static string DeclareAndBindQueueToExchange(IModel channel, out QueueingBasicConsumer consumer)
        {
            channel.ExchangeDeclare(ExchangeName, "fanout");
            var queueName = channel.QueueDeclare().QueueName;   // generates a queue-name for us
            channel.QueueBind(queueName, ExchangeName, "");
            consumer = new QueueingBasicConsumer(channel);
            return queueName;
        }
    }
}
