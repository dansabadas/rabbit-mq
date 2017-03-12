using DirectPaymentCardConsumer.RabbitMQ;

namespace PaymentCardConsumer
{
  public class DirectPaymentConsumer
  {
    public static void Run()
    {
      RabbitMQConsumerRpcDirectPayment client = new RabbitMQConsumerRpcDirectPayment();
      client.CreateConnection();
      client.ProcessMessages();
    }
  }
}
