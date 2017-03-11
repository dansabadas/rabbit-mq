using AccountsAuditConsumer.RabbitMQ;

namespace AccountsAuditConsumer
{
  public class AccountsAuditConsumer
  {
    static void Run()
    {
      RabbitMQConsumerAudit client = new RabbitMQConsumerAudit();
      client.CreateConnection();
      client.ProcessMessages();
    }
  }
}
