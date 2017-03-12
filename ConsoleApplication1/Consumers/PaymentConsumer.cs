using PaymentCardConsumer.RabbitMQ;

namespace PaymentCardConsumer
{
  public class PaymentConsumer
  {
    public static void Run()
    {
      RabbitMQConsumerPayment client = new RabbitMQConsumerPayment();
      client.CreateConnection();
      client.ProcessMessages();
    }
  }
}
