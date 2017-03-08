using RabbitMQ.Client;

namespace ConsoleApplication1.Common
{
    public static class RabbitMQConnectionFactory
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
    }
}
