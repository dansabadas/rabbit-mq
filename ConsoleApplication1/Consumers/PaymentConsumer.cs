using System.Threading.Tasks;

namespace Consumers
{
  public class PaymentConsumer
  {
    public static void Run()
    {
      RabbitMQConsumerPayment client = new RabbitMQConsumerPayment();
      client.CreateConnection();
      client.ProcessMessages();
    }

    public static void RunInParallel()
    {
      Task.Run(() => Run());
    }
  }
}
