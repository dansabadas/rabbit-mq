using System;
using System.Net;
using System.Web.Http;
using RabbitMQ.Examples;
using RabbitMQ.Core;

namespace Payments.Controllers
{
    public class DirectCardPaymentController : ApiController
    {
      [HttpPost]
      public IHttpActionResult MakePayment([FromBody] Payment payment)
      {
        string reply=null;

        try
        {
          RabbitMQConnectionFactory client = new RabbitMQConnectionFactory();
          client.CreateInstanceConnection();
          reply = client.MakePayment(payment);

          client.InstanceConnectionClose();
      }
      catch (Exception ex)
        {
          return StatusCode(HttpStatusCode.BadRequest);
        }

        return Ok(reply);
      }
    }
}
