namespace Consumers
{
  public class PurchaseOrderConsumer
  {
    public static void Run()
    {
      RabbitMQConsumerPurchaseOrder client = new RabbitMQConsumerPurchaseOrder();
      client.CreateConnection();
      client.ProcessMessages();
      client.Close();
    }
  }
}
