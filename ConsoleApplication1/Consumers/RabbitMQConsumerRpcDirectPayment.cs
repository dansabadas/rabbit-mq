using System;
using System.Globalization;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Examples;
using RabbitMQ.Core;

namespace DirectPaymentCardConsumer.RabbitMQ
{
    public class RabbitMQConsumerRpcDirectPayment
  {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private static QueueingBasicConsumer _consumer;
        private static Random _rnd;

        public void ProcessMessages()
        {
            while (true)
            {
                GetMessageFromQueue();
            }
        }

        private void GetMessageFromQueue()
        {
            string response = null;
            var ea = _consumer.Queue.Dequeue();
            var props = ea.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            Console.WriteLine("----------------------------------------------------------");

            try
            {
                response = MakePayment(ea);
                Console.WriteLine("Correlation ID = {0}", props.CorrelationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" ERROR : " + ex.Message);
                response = "";
            }
            finally
            {
                if (response != null)
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    _channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                }
                _channel.BasicAck(ea.DeliveryTag, false);
            }

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("");
        }

        private string MakePayment(BasicDeliverEventArgs ea)
        {
            var payment = (Payment)ea.Body.DeSerialize(typeof(Payment));
            var response = _rnd.Next(1000, 100000000).ToString(CultureInfo.InvariantCulture);
            Console.WriteLine("Payment -  {0} : £{1} : Auth Code <{2}> ", payment.CardNumber, payment.AmountToPay, response);

            return response;
        }

        public void CreateConnection()
        {
            _factory = RabbitMQConnectionFactory.GetFactory(isRemote: true);
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("rpc_queue", false, false, false, null);
            _channel.BasicQos(0, 1, false);
            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume("rpc_queue", false, _consumer);
            _rnd = new Random();
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
