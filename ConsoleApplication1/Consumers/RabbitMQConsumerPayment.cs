using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Examples;
using RabbitMQ.Core;

namespace Consumers
{
    public class RabbitMQConsumerPayment
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        
        private const string ExchangeName = "Topic_Exchange";
        private const string CardPaymentQueueName = "CardPaymentTopic_Queue";

        public void CreateConnection()
        {
            _factory = RabbitMQConnectionFactory.GetFactory(isRemote: true);
    }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessages()
        {
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <payment.cardpayment>");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(CardPaymentQueueName, 
                        true, false, false, null);

                    channel.QueueBind(CardPaymentQueueName, ExchangeName, 
                        "payment.cardpayment");

                    channel.BasicQos(0, 10, false);
                    Subscription subscription = new Subscription(channel, 
                        CardPaymentQueueName, false);
                    
                    while (true)
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();

                        var message = 
                            (Payment)deliveryArguments.Body.DeSerialize(typeof(Payment));

                        var routingKey = deliveryArguments.RoutingKey;

                        Console.WriteLine("--- Payment - Routing Key <{0}> : {1} : {2}", routingKey, message.CardNumber, message.AmountToPay);
                        subscription.Ack(deliveryArguments);
                    }
                }
            }
        }
    }
}
