using ConsoleApplication1.StandardQueue;
using Consumers;
using RabbitMQ.Examples;
using System;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      //MainStandardQueue.Run();

      //ProducerProgram.Run();
      //Consumer.RunInParallel();

      //Subscriber.RunInParallel();
      //Publisher.Run();

      PaymentConsumer.RunInParallel();
      AccountsAuditConsumer.RunInParallel();
      Console.ReadLine();
    }
  }
}
