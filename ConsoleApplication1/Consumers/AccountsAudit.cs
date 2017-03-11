using AccountsAuditConsumer.RabbitMQ;

namespace AccountsAuditConsumer
{
    class Program
    {
        static void Run()
        {
            RabbitMQConsumer client = new RabbitMQConsumer();
            client.CreateConnection();
            client.ProcessMessages();            
        }
    }
}
