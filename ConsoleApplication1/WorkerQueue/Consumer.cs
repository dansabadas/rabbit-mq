using System;
using RabbitMQ.Client;
using ConsoleApplication1.Common;
using System.Threading.Tasks;

namespace RabbitMQ.Examples
{
    public class Consumer
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        
        private const string QueueName = "WorkerQueue_Queue";

        public static void RunInParallel()
        {
            Task.Run(() => Receive(1));
            Task.Run(() => Receive(2));
            Console.ReadLine();
        }

        public static void Run(int threadId)
        {
            Receive(0);
            Console.ReadLine();
        }

        public static void Receive(int threadId)
        {
            _factory = RabbitMQConnectionFactory.GetFactory(isRemote: false);
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(QueueName, false, consumer); 

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();
                        var message = (Payment)ea.Body.DeSerialize(typeof(Payment));
                        channel.BasicAck(ea.DeliveryTag, false);

                        Console.WriteLine("{2} => ----- Payment Processed {0} : {1}", message.CardNumber, message.AmountToPay, threadId);
                    }
                }
            }
        }
    }
}
