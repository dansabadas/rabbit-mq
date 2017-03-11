using RabbitMQ.Client;
using RabbitMQ.Examples;
using System;
using System.Text;

namespace RabbitMQ.Core
{
  public class RabbitMQConnectionFactory
  {
    public static ConnectionFactory GetFactory(bool isRemote)
    {
      return isRemote
          ? new ConnectionFactory
          {
            HostName = "lark.rmq.cloudamqp.com",
            VirtualHost = "uatqkgsm",   // remote rabbitMQ server manadatory requires VirtualHost!
            UserName = "uatqkgsm",
            Password = "JilDhdpIdi8ug2sd2Jh23ZuL6rETlZ_w",
          }
          : new ConnectionFactory
          {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
          };
    }

    private static ConnectionFactory _factory;
    private static IConnection _connection;
    private static IModel _model;

    private const string ExchangeName = "Topic_Exchange";
    private const string CardPaymentQueueName = "CardPaymentTopic_Queue";
    private const string PurchaseOrderQueueName = "PurchaseOrderTopic_Queue";
    private const string AllQueueName = "AllTopic_Queue";

    private static void CreateConnection()
    {
      _factory = GetFactory(isRemote: true);

      _connection = _factory.CreateConnection();
      _model = _connection.CreateModel();
      _model.ExchangeDeclare(ExchangeName, "topic");

      _model.QueueDeclare(CardPaymentQueueName, true, false, false, null);
      _model.QueueDeclare(PurchaseOrderQueueName, true, false, false, null);
      _model.QueueDeclare(AllQueueName, true, false, false, null);

      _model.QueueBind(CardPaymentQueueName, ExchangeName, "payment.card");
      _model.QueueBind(PurchaseOrderQueueName, ExchangeName, "payment.purchaseorder");

      _model.QueueBind(AllQueueName, ExchangeName, "payment.*");
    }

    public void StaticConnectionClose()
    {
      _connection.Close();
    }

    private IConnection _instanceConnection;
    private IModel _channel;
    private string _replyQueueName;
    private QueueingBasicConsumer _consumer;

    public void CreateInstanceConnection() 
    {
      var factory = GetFactory(isRemote: true);
      _instanceConnection = factory.CreateConnection();
      _channel = _instanceConnection.CreateModel();

      _replyQueueName = _channel.QueueDeclare("rpc_reply", true, false, false, null);

      _consumer = new QueueingBasicConsumer(_channel);
      _channel.BasicConsume(_replyQueueName, true, _consumer);
    }

    public void InstanceConnectionClose()
    {
      _instanceConnection.Close();
    }

    public string MakePayment(Payment payment)
    {
      var corrId = Guid.NewGuid().ToString();
      var props = _channel.CreateBasicProperties();
      props.ReplyTo = _replyQueueName;
      props.CorrelationId = corrId;

      _channel.BasicPublish("", "rpc_queue", props, payment.Serialize());

      while (true)
      {
        var ea = _consumer.Queue.Dequeue();

        if (ea.BasicProperties.CorrelationId != corrId) continue;

        var authCode = Encoding.UTF8.GetString(ea.Body);
        return authCode;
      }
    }
  }
}
