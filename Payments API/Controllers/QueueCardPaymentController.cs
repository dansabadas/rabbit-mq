using System;
using System.Net;
using System.Web.Http;
using RabbitMQ.Core;
using RabbitMQ.Examples;

namespace Payments.Controllers
{
  public class QueueCardPaymentController : ApiController
  {
    [HttpPost]
    public IHttpActionResult MakePayment([FromBody] Payment payment)
    {
      try
      {
        RabbitMQConnectionFactory client = new RabbitMQConnectionFactory();
        client.SendPayment(payment);
        client.StaticConnectionClose();
      }
      catch (Exception)
      {
        return StatusCode(HttpStatusCode.BadRequest);
      }

      return Ok(payment);
    }
  }
}
