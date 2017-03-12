using System.Threading.Tasks;

namespace Consumers
{
  public class AccountsAuditConsumer
  {
    public static void Run()
    {
      RabbitMQConsumerAudit client = new RabbitMQConsumerAudit();
      client.CreateConnection();
      client.ProcessMessages();
    }

    public static void RunInParallel()
    {
      Task.Run(() => Run());
    }
  }
}
